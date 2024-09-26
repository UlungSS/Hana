using Hana.Features.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hana.Features.Abstractions;

public abstract class FeatureBase(IModule module) : IFeature
{

    public IModule Module { get; } = module;

    public IServiceCollection Services => Module.Services;

    public virtual void Configure()
    {
    }

    public virtual void ConfigureHostedServices()
    {
    }

    public virtual void Apply()
    {
    }

    protected void ConfigureHostedService<T>(int priority = 0) where T : class, IHostedService
    {
        Module.ConfigureHostedService<T>(priority);
    }
}