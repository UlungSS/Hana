namespace Hana.Expressions.Contracts;

public interface IWellKnownTypeRegistry
{
    void RegisterType(Type type, string alias);

    bool TryGetAlias(Type type, out string alias);

    bool TryGetType(string alias, out Type type);

    IEnumerable<Type> ListTypes();
}