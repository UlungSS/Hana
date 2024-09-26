using Hana.Expressions.Contracts;
using Hana.Extensions;

namespace Hana.Expressions.Extensions;


public static class WellKnowTypeRegistryExtensions
{
    public static void RegisterType<T>(this IWellKnownTypeRegistry registry, string alias) => registry.RegisterType(typeof(T), alias);

    public static bool TryGetTypeOrDefault(this IWellKnownTypeRegistry registry, string alias, out Type type)
    {
        if (registry.TryGetType(alias, out type))
            return true;

        var t = Type.GetType(alias);

        if (t == null)
            return false;

        type = t;
        return true;
    }

    public static string GetAliasOrDefault(this IWellKnownTypeRegistry registry, Type type) =>
     registry.TryGetAlias(type, out var alias) ? alias : type.GetSimpleAssemblyQualifiedName();

    public static Type GetTypeOrDefault(this IWellKnownTypeRegistry registry, string alias) => registry.TryGetType(alias, out var type) ? type : Type.GetType(alias) ?? typeof(object);
}