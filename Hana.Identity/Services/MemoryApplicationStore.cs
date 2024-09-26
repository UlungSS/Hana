using Hana.Common.Services;
using Hana.Identity.Contracts;
using Hana.Identity.Entities;
using Hana.Identity.Models;

namespace Hana.Identity.Services;


public class MemoryApplicationStore(MemoryStore<Application> store) : IApplicationStore
{
    private readonly MemoryStore<Application> _store = store;

    public Task SaveAsync(Application application, CancellationToken cancellationToken = default)
    {
        _store.Save(application, x => x.Id);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(ApplicationFilter filter, CancellationToken cancellationToken = default)
    {
        var ids = _store.Query(query => Filter(query, filter)).Select(x => x.Id).Distinct().ToList();
        _store.DeleteWhere(x => ids.Contains(x.Id));
        return Task.CompletedTask;
    }

    public Task<Application?> FindAsync(ApplicationFilter filter, CancellationToken cancellationToken = default)
    {
        var result = _store.Query(query => Filter(query, filter)).FirstOrDefault();
        return Task.FromResult(result);
    }

    private static IQueryable<Application> Filter(IQueryable<Application> queryable, ApplicationFilter filter) => filter.Apply(queryable);
}