using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

using Hana.Features.Services;
using Hana.Features;

namespace Hana.Extensions;

public static class ModuleExtensions
{
    private static readonly IDictionary<IServiceCollection, IModule> Modules = new ConcurrentDictionary<IServiceCollection, IModule>();

    public static IModule AddHana(this IServiceCollection services, Action<IModule>? configure = default)
    {
        var module = services.GetOrCreateModule();
        module.Configure<AppFeature>(app => app.Configurator = configure);
        module.Apply();

        return module;
    }

    public static IModule ConfigureHana(this IServiceCollection services, Action<IModule>? configure = default)
    {
        var module = services.GetOrCreateModule();

        if (configure != null)
            module.Configure<AppFeature>(app => app.Configurator += configure);

        return module;
    }

    private static IModule GetOrCreateModule(this IServiceCollection services)
    {
        if (Modules.TryGetValue(services, out var module))
            return module;

        module = services.CreateModule();

        Modules[services] = module;
        return module;
    }
}