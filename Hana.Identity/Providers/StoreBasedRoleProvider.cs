using Hana.Identity.Contracts;
using Hana.Identity.Entities;
using Hana.Identity.Models;

using JetBrains.Annotations;

namespace Hana.Identity.Providers;


[PublicAPI]
public class StoreBasedRoleProvider(IRoleStore roleStore) : IRoleProvider
{
    private readonly IRoleStore _roleStore = roleStore;

    public async ValueTask<IEnumerable<Role>> FindManyAsync(RoleFilter filter, CancellationToken cancellationToken = default)
    {
        return await _roleStore.FindManyAsync(filter, cancellationToken);
    }
}