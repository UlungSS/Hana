using Hana.Identity.Entities;
using Hana.Identity.Models;

namespace Hana.Identity.Contracts;


public interface IRoleStore
{
    Task AddAsync(Role role, CancellationToken cancellationToken = default);

    Task DeleteAsync(RoleFilter filter, CancellationToken cancellationToken = default);

    Task SaveAsync(Role role, CancellationToken cancellationToken = default);

    Task<Role?> FindAsync(RoleFilter filter, CancellationToken cancellationToken = default);

    Task<IEnumerable<Role>> FindManyAsync(RoleFilter filter, CancellationToken cancellationToken = default);
}