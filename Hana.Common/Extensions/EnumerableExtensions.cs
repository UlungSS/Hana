using Hana.Common.Models;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace Elsa.Extensions;

[PublicAPI]
public static class EnumerableExtensions
{
    public static Page<TTarget> Paginate<T, TTarget>(this IEnumerable<T> enumerable, Func<T, TTarget> projection, PageArgs? pageArgs = default)
    {
        var items = enumerable.ToList();
        var count = items.Count;

        if (pageArgs?.Offset != null) items = items.Skip(pageArgs.Offset.Value).ToList();
        if (pageArgs?.Limit != null) items = items.Take(pageArgs.Limit.Value).ToList();

        var results = items.Select(projection).ToList();
        return Page.Of(results, count);
    }

    public static Page<T> Paginate<T>(this IEnumerable<T> enumerable, PageArgs? pageArgs = default)
    {
        var items = enumerable.ToList();
        var count = items.Count;

        if (pageArgs?.Offset != null) items = items.Skip(pageArgs.Offset.Value).ToList();
        if (pageArgs?.Limit != null) items = items.Take(pageArgs.Limit.Value).ToList();

        var results = items.ToList();
        return Page.Of(results, count);
    }

    public static bool IsEqualTo<T>(this IEnumerable<T> list1, IEnumerable<T> list2) => list1.OrderBy(g => g).SequenceEqual(list2.OrderBy(g => g));

}