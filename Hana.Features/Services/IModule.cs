using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hana.Features.Services;


public interface IModule
{
    IServiceCollection Services { get; }

    IDictionary<object, object> Properties { get; }

    bool HasFeature<T>() where T : class, IFeature;

    bool HasFeature(Type featureType);

    T Configure<T>(Action<T>? configure = default) where T : class, IFeature;

    T Configure<T>(Func<IModule, T> factory, Action<T>? configure = default) where T : class, IFeature;

    IModule ConfigureHostedService<T>(int priority = 0) where T : class, IHostedService;

    IModule ConfigureHostedService(Type hostedServiceType, int priority = 0);

    void Apply();
}