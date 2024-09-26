using System.Collections.Concurrent;

using Hana.Common.Entities;

namespace Hana.Common.Services;


public class MemoryStore<TEntity>
{
    private IDictionary<string, TEntity> Entities { get; set; } = new ConcurrentDictionary<string, TEntity>();

    public void Add(TEntity entity, Func<TEntity, string> idAccessor) => Entities.Add(idAccessor(entity), entity);

    public void AddMany(IEnumerable<TEntity> entities, Func<TEntity, string> idAccessor)
    {
        foreach (var entity in entities)
            Add(entity, idAccessor);
    }

    public void Save(TEntity entity, Func<TEntity, string> idAccessor) => Entities[idAccessor(entity)] = entity;

    public void SaveMany(IEnumerable<TEntity> entities, Func<TEntity, string> idAccessor)
    {
        foreach (var entity in entities)
            Save(entity, idAccessor);
    }

    public void Update(TEntity entity, Func<TEntity, string> idAccessor) => Entities[idAccessor(entity)] = entity;

    public TEntity? Find(Func<TEntity, bool> predicate) => Entities.Values.Where(predicate).FirstOrDefault();

    public IEnumerable<TEntity> FindMany(Func<TEntity, bool> predicate) => Entities.Values.Where(predicate);

    public IEnumerable<TEntity> FindMany<TKey>(Func<TEntity, bool> predicate, Func<TEntity, TKey> orderBy, OrderDirection orderDirection = OrderDirection.Ascending)
    {
        var query = Entities.Values.Where(predicate);

        query = orderDirection switch
        {
            OrderDirection.Ascending => query.OrderBy(orderBy),
            OrderDirection.Descending => query.OrderByDescending(orderBy),
            _ => query.OrderBy(orderBy)
        };

        return query;
    }

    public IEnumerable<TEntity> List() => Entities.Values;

    public bool Delete(string id) => Entities.Remove(id);

    public long DeleteWhere(Func<TEntity, bool> predicate)
    {
        var query =
            from entry in Entities
            where predicate(entry.Value)
            select entry;

        var entries = query.ToList();
        foreach (var entry in entries)
            Entities.Remove(entry);

        return entries.Count;
    }

    public long DeleteMany(IEnumerable<string> ids)
    {
        var count = 0;
        foreach (var id in ids)
        {
            count++;
            Entities.Remove(id);
        }

        return count;
    }

    public long DeleteMany(IEnumerable<TEntity> entities, Func<TEntity, string> idAccessor)
    {
        var count = 0;
        var list = entities.ToList();

        foreach (var entity in list)
        {
            count++;
            var id = idAccessor(entity);
            Entities.Remove(id);
        }

        return count;
    }

    public IEnumerable<TEntity> Query(Func<IQueryable<TEntity>, IQueryable<TEntity>> query)
    {
        var queryable = Entities.Values.AsQueryable();
        return query(queryable);
    }

    public bool Any(Func<TEntity, bool> predicate) => Entities.Values.Any(predicate);

    public long Count<TProperty>(Func<TEntity, bool> predicate, Func<TEntity, TProperty> propertySelector)
    {
        return Entities.Values
            .DistinctBy(propertySelector)
            .Count(predicate);
    }
}