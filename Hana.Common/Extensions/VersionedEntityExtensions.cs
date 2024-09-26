using System.Linq.Expressions;

using Hana.Common.Entities;
using Hana.Common.Models;

using LinqKit;

// ReSharper disable once CheckNamespace
namespace Hana.Extensions;

public static class VersionedEntityExtensions
{
    public static bool WithVersion(this VersionedEntity entity, VersionOptions versionOptions)
    {
        var isPublished = entity.IsPublished;
        var isLatest = entity.IsLatest;
        var version = entity.Version;

        if (versionOptions.IsDraft)
            return !isPublished;
        if (versionOptions.IsLatest)
            return isLatest;
        if (versionOptions.IsPublished)
            return isPublished;
        if (versionOptions.IsLatestOrPublished)
            return isPublished || isLatest;
        if (versionOptions.IsLatestAndPublished)
            return isPublished && isLatest;
        if (versionOptions.AllVersions)
            return true;
        if (versionOptions.Version > 0)
            return version == versionOptions.Version;
        return true;
    }
    public static IEnumerable<T> WithVersion<T>(
        this IEnumerable<T> enumerable,
        VersionOptions versionOptions) where T : VersionedEntity =>
        enumerable.Where(x => x.WithVersion(versionOptions)).OrderByDescending(x => x.Version);

    public static IQueryable<T> WithVersion<T>(this IQueryable<T> query, VersionOptions versionOptions) where T : VersionedEntity
    {
        if (versionOptions.IsDraft)
            return query.Where(x => !x.IsPublished);
        if (versionOptions.IsLatest)
            return query.Where(x => x.IsLatest);
        if (versionOptions.IsPublished)
            return query.Where(x => x.IsPublished);
        if (versionOptions.IsLatestOrPublished)
            return query.Where(x => x.IsPublished || x.IsLatest);
        if (versionOptions.IsLatestAndPublished)
            return query.Where(x => x.IsPublished && x.IsLatest);
        if (versionOptions.Version > 0)
            return query.Where(x => x.Version == versionOptions.Version);

        return query;
    }

    public static Expression<Func<T, bool>> WithVersion<T>(this Expression<Func<T, bool>> expression, VersionOptions versionOptions) where T : VersionedEntity
    {
        if (versionOptions.IsDraft)
            return expression.And(x => !x.IsPublished);
        if (versionOptions.IsLatest)
            return expression.And(x => x.IsLatest);
        if (versionOptions.IsPublished)
            return expression.And(x => x.IsPublished);
        if (versionOptions.IsLatestOrPublished)
            return expression.And(x => x.IsPublished || x.IsLatest);
        if (versionOptions.IsLatestAndPublished)
            return expression.And(x => x.IsPublished && x.IsLatest);
        if (versionOptions.Version > 0)
            return expression.And(x => x.Version == versionOptions.Version);

        return expression;
    }
}