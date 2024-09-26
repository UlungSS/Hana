using Hana.Identity.Entities;
using Hana.Identity.Models;

namespace Hana.Identity.Contracts;

public interface IUserStore
{
    Task SaveAsync(User user, CancellationToken cancellationToken = default);
    Task DeleteAsync(UserFilter filter, CancellationToken cancellationToken = default);
    Task<User?> FindAsync(UserFilter filter, CancellationToken cancellationToken = default);
}