using Hana.Expressions.Contracts;
using Hana.Expressions.Options;
using Microsoft.Extensions.Options;

namespace Hana.Expressions.Services;


public class WellKnownTypeRegistry : IWellKnownTypeRegistry
{
    private readonly Dictionary<string, Type> _aliasTypeDictionary = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<Type, string> _typeAliasDictionary = new();

    public static IWellKnownTypeRegistry CreateDefault()
    {
        var registry = new WellKnownTypeRegistry();
        return registry;
    }

    public WellKnownTypeRegistry(IOptions<ExpressionOptions> expressionOptions)
    {
        foreach (var entry in expressionOptions.Value.AliasTypeDictionary)
            RegisterType(entry.Value, entry.Key);
    }

    public WellKnownTypeRegistry()
    {
    }

    public void RegisterType(Type type, string alias)
    {
        _typeAliasDictionary[type] = alias;
        _aliasTypeDictionary[alias] = type;

        if (type.IsPrimitive || type.IsValueType && Nullable.GetUnderlyingType(type) == null)
        {
            var nullableType = typeof(Nullable<>).MakeGenericType(type);
            var nullableAlias = alias + "?";
            _typeAliasDictionary[nullableType] = nullableAlias;
            _aliasTypeDictionary[nullableAlias] = nullableType;
        }
    }

    public bool TryGetAlias(Type type, out string alias) => _typeAliasDictionary.TryGetValue(type, out alias!);

    public bool TryGetType(string alias, out Type type) => _aliasTypeDictionary.TryGetValue(alias, out type!);

    public IEnumerable<Type> ListTypes() => _typeAliasDictionary.Keys;
}