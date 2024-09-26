using Hana.Common.Contracts;
using Hana.Common.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Hana.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMemoryStore<TEntity, TStore>(this IServiceCollection services) where TStore : class
    {
        services.TryAddSingleton<MemoryStore<TEntity>>();
        services.TryAddScoped<TStore>();
        return services;
    }

    public static IServiceCollection AddSerializationOptionsConfigurator<T>(this IServiceCollection services) where T : class, ISerializationOptionsConfigurator
    {
        services.AddSingleton<ISerializationOptionsConfigurator, T>();
        return services;
    }
}