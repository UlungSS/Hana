// ReSharper disable once CheckNamespace
namespace Hana.Extensions;

public static class CollectionExtensions
{
     public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> source)
    {
        foreach (var item in source) target.Add(item);
    }

    public static void AddRange<T>(this ICollection<T> target, params T[] source) => AddRange(target, source.AsEnumerable());

    public static void RemoveWhere<T>(this ICollection<T> collection, Func<T, bool> predicate)
    {
        var itemsToRemove = collection.Where(predicate).ToList();
        foreach (var item in itemsToRemove) collection.Remove(item);
    }
}