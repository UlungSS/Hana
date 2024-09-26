using Hana.Identity.Entities;
using Hana.Identity.Models;

namespace Hana.Identity.Contracts;

public interface IUserProvider
{
    Task<User?> FindAsync(UserFilter filter, CancellationToken cancellationToken = default);
}