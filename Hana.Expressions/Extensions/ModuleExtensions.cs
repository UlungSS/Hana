using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

using Hana.Expressions.Contracts;
using Hana.Expressions.Features;
using Hana.Expressions.Options;
using Hana.Features.Services;

// ReSharper disable once CheckNamespace
namespace Hana.Extensions;


[PublicAPI]
public static class ModuleExtensions
{
    public static IModule UseExpressions(this IModule module, Action<ExpressionsFeature>? configure = default)
    {
        module.Configure(configure);
        return module;
    }

    public static IServiceCollection AddExpressionDescriptorProvider<T>(this IServiceCollection services) where T : class, IExpressionDescriptorProvider
    {
        return services.AddSingleton<IExpressionDescriptorProvider, T>();
    }

    public static IModule AddTypeAlias<T>(this IModule module, string alias)
    {
        module.Services.Configure<ExpressionOptions>(options => options.AddTypeAlias<T>(alias));
        return module;
    }
}