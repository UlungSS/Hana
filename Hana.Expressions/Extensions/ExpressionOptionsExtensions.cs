using Hana.Expressions.Options;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace Hana.Extensions;


[PublicAPI]
public static class ExpressionOptionsExtensions
{
    /// <summary>
    /// Register type <typeparamref name="T"/> with the specified alias.
    /// </summary>
    public static void AddTypeAlias<T>(this ExpressionOptions options) => options.RegisterTypeAlias(typeof(T), typeof(T).Name);
    public static void AddTypeAlias<T>(this ExpressionOptions options, string alias) => options.RegisterTypeAlias(typeof(T), alias);
}