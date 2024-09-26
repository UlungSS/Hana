using Hana.Identity.Entities;

namespace Hana.Identity.Models;

public class RoleFilter
{
    public string? Id { get; set; }

    public ICollection<string>? Ids { get; set; }

    public IQueryable<Role> Apply(IQueryable<Role> queryable)
    {
        var filter = this;
        if (filter.Id != null) queryable = queryable.Where(x => x.Id == filter.Id);
        if (filter.Ids != null) queryable = queryable.Where(x => filter.Ids.Contains(x.Id));

        return queryable;
    }
}