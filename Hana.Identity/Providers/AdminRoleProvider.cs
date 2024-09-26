using Hana.Identity.Contracts;
using Hana.Identity.Entities;
using Hana.Identity.Models;

namespace Hana.Identity.Providers;

public class AdminRoleProvider : IRoleProvider
{
    public ValueTask<IEnumerable<Role>> FindManyAsync(RoleFilter filter, CancellationToken cancellationToken = default)
    {
        var adminRole = new Role
        {
            Id = "admin",
            Name = "admin",
            Permissions = { "*" }
        };

        return new(new[] { adminRole });
    }
}