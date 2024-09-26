using Hana.Identity.Entities;

namespace Hana.Identity.Models;

public class UserFilter
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public IQueryable<User> Apply(IQueryable<User> queryable)
    {
        var filter = this;
        if (filter.Id != null) queryable = queryable.Where(x => x.Id == filter.Id);
        if (filter.Name != null) queryable = queryable.Where(x => x.Name == filter.Name);

        return queryable;
    }
}