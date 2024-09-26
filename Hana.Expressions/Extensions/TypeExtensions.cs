using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Hana.Expressions.Models;

// ReSharper disable once CheckNamespace
namespace Hana.Extensions;


public static class TypeExtensions
{
    private static readonly ConcurrentDictionary<Type, string> SimpleAssemblyQualifiedTypeNameCache = new();

    public static string GetSimpleAssemblyQualifiedName(this Type type)
    {
        return type is null
            ? throw new ArgumentNullException(nameof(type))
            : SimpleAssemblyQualifiedTypeNameCache.GetOrAdd(type, GetSimplifiedName);
    }

    public static object? GetDefaultValue(this Type type) => type.IsClass ? null : Activator.CreateInstance(type);

    public static Type GetEnumerableElementType(this Type type)
    {
        if (type.IsArray)
            return type.GetElementType()!;

        var elementType = FindIEnumerable(type);
        return elementType == null ? type : elementType.GetGenericArguments()[0];
    }


    [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
    private static Type? FindIEnumerable([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] Type? sequenceType)
    {
        if (sequenceType == null || sequenceType == typeof(string))
            return null;

        if (sequenceType.IsArray)
            return typeof(IEnumerable<>).MakeGenericType(sequenceType.GetElementType()!);

        if (sequenceType.IsGenericType)
        {
            foreach (var arg in sequenceType.GetGenericArguments())
            {
                var enumerable = typeof(IEnumerable<>).MakeGenericType(arg);
                if (enumerable.IsAssignableFrom(sequenceType))
                    return enumerable;
            }
        }

        var interfaces = sequenceType.GetInterfaces();

        if (interfaces is { Length: > 0 })
        {
            foreach (var interfaceType in interfaces)
            {
                var enumerable = FindIEnumerable(interfaceType);
                if (enumerable != null) return enumerable;
            }
        }
        if (sequenceType.BaseType != null && sequenceType.BaseType != typeof(object))
            return FindIEnumerable(sequenceType.BaseType);

        return null;
    }

    public static string GetFriendlyTypeName(this Type type, Brackets brackets)
    {
        if (!type.IsGenericType)
            return type.FullName!;

        var sb = new StringBuilder();
        sb.Append(type.Namespace);
        sb.Append('.');
        sb.Append(type.Name[..type.Name.IndexOf('`')]);
        sb.Append(brackets.Open);
        var genericArgs = type.GetGenericArguments();
        for (var i = 0; i < genericArgs.Length; i++)
        {
            if (i > 0)
                sb.Append(", ");
            sb.Append(GetFriendlyTypeName(genericArgs[i], brackets));
        }

        sb.Append(brackets.Close);
        return sb.ToString();
    }

    private static string GetSimplifiedName(Type type)
    {
        var assemblyName = type.Assembly.GetName().Name;

        if (type.IsGenericType)
        {
            var genericTypeName = type.GetGenericTypeDefinition().FullName!;
            var backtickIndex = genericTypeName.IndexOf('`');
            var typeNameWithoutArity = genericTypeName[..backtickIndex];
            var arity = genericTypeName[backtickIndex..];

            var genericArguments = type.GetGenericArguments();
            var simplifiedGenericArguments = genericArguments.Select(GetSimplifiedName);

            return $"{typeNameWithoutArity}{arity}[[{string.Join("],[", simplifiedGenericArguments)}]], {assemblyName}";
        }

        var typeName = type.FullName;
        return $"{typeName}, {assemblyName}";
    }
}