using Hana.Common.Services;
using Hana.Identity.Contracts;
using Hana.Identity.Entities;
using Hana.Identity.Models;

namespace Hana.Identity.Services;


public class MemoryUserStore(MemoryStore<User> store) : IUserStore
{
    private readonly MemoryStore<User> _store = store;

    public Task SaveAsync(User user, CancellationToken cancellationToken = default)
    {
        _store.Save(user, x => x.Id);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(UserFilter filter, CancellationToken cancellationToken = default)
    {
        var ids = _store.Query(query => Filter(query, filter)).Select(x => x.Id).Distinct().ToList();
        _store.DeleteWhere(x => ids.Contains(x.Id));
        return Task.CompletedTask;
    }

    public Task<User?> FindAsync(UserFilter filter, CancellationToken cancellationToken = default)
    {
        var result = _store.Query(query => Filter(query, filter)).FirstOrDefault();
        return Task.FromResult(result);
    }

    private static IQueryable<User> Filter(IQueryable<User> queryable, UserFilter filter) => filter.Apply(queryable);
}