using Hana.Common.Services;
using Hana.Identity.Contracts;
using Hana.Identity.Entities;
using Hana.Identity.Models;

namespace Hana.Identity.Services;


public class MemoryRoleStore(MemoryStore<Role> store) : IRoleStore
{
    private readonly MemoryStore<Role> _store = store;

    public Task AddAsync(Role role, CancellationToken cancellationToken = default)
    {
        _store.Save(role, x => x.Id);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(RoleFilter filter, CancellationToken cancellationToken = default)
    {
        var ids = _store.Query(query => Filter(query, filter)).Select(x => x.Id).Distinct().ToList();
        _store.DeleteWhere(x => ids.Contains(x.Id));
        return Task.CompletedTask;
    }

    public Task SaveAsync(Role role, CancellationToken cancellationToken = default)
    {
        _store.Save(role, x => x.Id);
        return Task.CompletedTask;
    }

    public Task<Role?> FindAsync(RoleFilter filter, CancellationToken cancellationToken = default)
    {
        var result = _store.Query(query => Filter(query, filter)).FirstOrDefault();
        return Task.FromResult(result);
    }

    public Task<IEnumerable<Role>> FindManyAsync(RoleFilter filter, CancellationToken cancellationToken = default)
    {
        var result = _store.Query(query => Filter(query, filter)).ToList().AsEnumerable();
        return Task.FromResult(result);
    }

    private static IQueryable<Role> Filter(IQueryable<Role> queryable, RoleFilter filter) => filter.Apply(queryable);
}