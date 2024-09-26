// ReSharper disable once CheckNamespace
namespace Hana.Extensions;

public static class TypeExtensions
{
    public static bool IsGenericType(this Type type, Type genericType) => type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
    
    public static bool IsNullableType(this Type type) => type.IsGenericType(typeof(Nullable<>));
    
    public static Type GetTypeOfNullable(this Type type) => type.GenericTypeArguments[0];

    public static bool IsCollectionType(this Type type)
    {
        if (!type.IsGenericType)
            return false;
        
        var elementType = type.GenericTypeArguments[0];
        var collectionType = typeof(ICollection<>).MakeGenericType(elementType);
        var listType = typeof(IList<>).MakeGenericType(elementType);
        return collectionType.IsAssignableFrom(type) || listType.IsAssignableFrom(type);
    }

    public static Type MakeCollectionType(this Type type) => typeof(ICollection<>).MakeGenericType(type);

    public static Type GetCollectionElementType(this Type type) => type.GenericTypeArguments[0];
}