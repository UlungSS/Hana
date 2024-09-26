using Hana.Identity.Entities;
using Hana.Identity.Models;

namespace Hana.Identity.Contracts;

public interface IRoleProvider
{
    ValueTask<IEnumerable<Role>> FindManyAsync(RoleFilter filter, CancellationToken cancellationToken = default);
}