using Hana.Features.Services;
using Hana.Core.Contracts;
using Hana.Core.Features;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Hana.Extensions;

public static class ModuleExtensions
{
    public static IModule UseHanaCore(this IModule configuration, Action<CoreFeature>? configure = default)
    {
        configuration.Configure(configure);
        return configuration;
    }

}