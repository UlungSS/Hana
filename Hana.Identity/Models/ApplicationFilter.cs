using Hana.Identity.Entities;

namespace Hana.Identity.Models;

public class ApplicationFilter
{
    public string? Id { get; set; }
    public string? ClientId { get; set; }
    public string? Name { get; set; }
    public IQueryable<Application> Apply(IQueryable<Application> queryable)
    {
        var filter = this;
        if (filter.Id != null) queryable = queryable.Where(x => x.Id == filter.Id);
        if (filter.ClientId != null) queryable = queryable.Where(x => x.ClientId == filter.ClientId);
        if (filter.Name != null) queryable = queryable.Where(x => x.Name == filter.Name);

        return queryable;
    }
}