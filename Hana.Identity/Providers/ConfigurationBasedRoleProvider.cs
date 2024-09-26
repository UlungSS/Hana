using Hana.Identity.Contracts;
using Hana.Identity.Entities;
using Hana.Identity.Models;
using Hana.Identity.Options;

using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Hana.Identity.Providers;


[PublicAPI]
public class ConfigurationBasedRoleProvider(IOptions<RolesOptions> options) : IRoleProvider
{
    private readonly IOptions<RolesOptions> _options = options;

    public ValueTask<IEnumerable<Role>> FindManyAsync(RoleFilter filter, CancellationToken cancellationToken = default)
    {
        var rolesQueryable = _options.Value.Roles.AsQueryable();
        var roles = filter.Apply(rolesQueryable).ToList();
        return new(roles);
    }
}