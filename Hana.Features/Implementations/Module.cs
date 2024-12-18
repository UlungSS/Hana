using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

using Hana.Extensions;
using Hana.Features.Attributes;
using Hana.Features.Contracts;
using Hana.Features.Models;
using Hana.Features.Services;

namespace Hana.Features.Implementations;

public class Module(IServiceCollection services) : IModule
{
    private sealed record HostedServiceDescriptor(int Order, Type Type);

    private Dictionary<Type, IFeature> _features = [];
    private readonly HashSet<IFeature> _configuredFeatures = [];
    private readonly List<HostedServiceDescriptor> _hostedServiceDescriptors = [];

    public IServiceCollection Services { get; } = services;

    public IDictionary<object, object> Properties { get; } = new Dictionary<object, object>();

    public bool HasFeature<T>() where T : class, IFeature
    {
        return HasFeature(typeof(T));
    }

    public bool HasFeature(Type featureType)
    {
        return _features.ContainsKey(featureType);
    }

    public T Configure<T>(Action<T>? configure = default) where T : class, IFeature
     => Configure(module => (T)Activator.CreateInstance(typeof(T), module)!, configure);

    public T Configure<T>(Func<IModule, T> factory, Action<T>? configure = default) where T : class, IFeature
    {
        if (!_features.TryGetValue(typeof(T), out var feature))
        {
            feature = factory(this);
            _features[typeof(T)] = feature;
        }

        configure?.Invoke((T)feature);

        if (!_isApplying)
            return (T)feature;

        var dependencies = GetDependencyTypes(feature.GetType()).ToHashSet();
        foreach (var dependency in dependencies.Select(GetOrCreateFeature))
            ConfigureFeature(dependency);

        ConfigureFeature(feature);
        return (T)feature;
    }

    public IModule ConfigureHostedService<T>(int priority = 0) where T : class, IHostedService
    {
        return ConfigureHostedService(typeof(T), priority);
    }

    public IModule ConfigureHostedService(Type hostedServiceType, int priority = 0)
    {
        _hostedServiceDescriptors.Add(new HostedServiceDescriptor(priority, hostedServiceType));
        return this;
    }

    private bool _isApplying;

    public void Apply()
    {
        _isApplying = true;
        var featureTypes = GetFeatureTypes();
        _features = featureTypes.ToDictionary(featureType => featureType, featureType => _features.TryGetValue(featureType, out var existingFeature) ? existingFeature : (IFeature)Activator.CreateInstance(featureType, this)!);

        // Iterate over a copy of the features to avoid concurrent modification exceptions.
        foreach (var feature in _features.Values.ToList())
        {
            // This will cause additional features to be added to _features.
            ConfigureFeature(feature);
        }

        // Filter out features that depend on other features that are not installed.
        _features = ExcludeFeaturesWithMissingDependencies(_features.Values).ToDictionary(x => x.GetType(), x => x);

        // Add hosted services in order of priority.
        foreach (var hostedServiceDescriptor in _hostedServiceDescriptors.OrderBy(x => x.Order))
            Services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IHostedService), hostedServiceDescriptor.Type));

        // Make sure to use the complete list of features when applying them.
        foreach (var feature in _features.Values)
            feature.Apply();

        // Add a registry of enabled features to the service collection for client applications to reflect on what features are installed.
        var registry = new InstalledFeatureRegistry();
        foreach (var feature in _features.Values)
        {
            var type = feature.GetType();
            var name = type.Name.Replace("Feature", string.Empty);
            var ns = "Hana";
            var displayName = type.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? name;
            var description = type.GetCustomAttribute<DescriptionAttribute>()?.Description;
            registry.Add(new FeatureDescriptor(name, ns, displayName, description));
        }

        Services.AddSingleton<IInstalledFeatureRegistry>(registry);
    }

    private IEnumerable<IFeature> ExcludeFeaturesWithMissingDependencies(IEnumerable<IFeature> features)
    {
        return
            from feature in features
            let featureType = feature.GetType()
            let dependencyOfAttributes = featureType.GetCustomAttributes<DependencyOfAttribute>().ToList()
            let missingDependencies = dependencyOfAttributes.Where(x => !_features.ContainsKey(x.Type)).ToList()
            where missingDependencies.Count == 0
            select feature;
    }

    private void ConfigureFeature(IFeature feature)
    {
        if (_configuredFeatures.Contains(feature))
            return;

        feature.Configure();
        feature.ConfigureHostedServices();
        _features[feature.GetType()] = feature;
        _configuredFeatures.Add(feature);
    }

    private IFeature GetOrCreateFeature(Type featureType)
    {
        return _features.TryGetValue(featureType, out var existingFeature) ? existingFeature : (IFeature)Activator.CreateInstance(featureType, this)!;
    }

    private HashSet<Type> GetFeatureTypes()
    {
        var featureTypes = _features.Keys.ToHashSet();
        var featureTypesWithDependencies = featureTypes.Concat(featureTypes.SelectMany(GetDependencyTypes)).ToHashSet();
        return featureTypesWithDependencies.TSort(x => x.GetCustomAttributes<DependsOnAttribute>().Select(dependsOn => dependsOn.Type)).ToHashSet();
    }

    // Recursively get dependency types.
    private IEnumerable<Type> GetDependencyTypes(Type type)
    {
        var dependencies = type.GetCustomAttributes<DependsOnAttribute>().Select(dependsOn => dependsOn.Type).ToList();
        return dependencies.Concat(dependencies.SelectMany(GetDependencyTypes));
    }
}