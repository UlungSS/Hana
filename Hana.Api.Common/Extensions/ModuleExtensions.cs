using System.Reflection;
using Hana.Features.Services;
using FastEndpoints;

// ReSharper disable once CheckNamespace
namespace Hana.Extensions;

public static class ModuleExtensions
{
    private static readonly object FastEndpointsAssembliesKey = new();

    public static IModule AddFastEndpointsAssembly(this IModule module, Assembly assembly)
    {
        var assemblies = module.Properties.GetOrAdd(FastEndpointsAssembliesKey, () => new HashSet<Assembly>());
        assemblies.Add(assembly);
        return module;
    }

    public static IModule AddFastEndpointsAssembly<T>(this IModule module) => module.AddFastEndpointsAssembly(typeof(T));

    public static IModule AddFastEndpointsAssembly(this IModule module, Type markerType) => module.AddFastEndpointsAssembly(markerType.Assembly);

    public static IEnumerable<Assembly> GetFastEndpointsAssembliesFromModule(this IModule module) => module.Properties.GetOrAdd(FastEndpointsAssembliesKey, () => new HashSet<Assembly>());

    public static IModule AddFastEndpointsFromModule(this IModule module)
    {
        var assemblies = module.GetFastEndpointsAssembliesFromModule().ToList();

        module.Services.AddFastEndpoints(options =>
        {
            options.DisableAutoDiscovery = true;
            options.Assemblies = assemblies;
        });

        return module;
    }
}