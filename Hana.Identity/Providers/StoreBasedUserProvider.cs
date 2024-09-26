using Hana.Identity.Contracts;
using Hana.Identity.Entities;
using Hana.Identity.Models;

using JetBrains.Annotations;

namespace Hana.Identity.Providers;


[PublicAPI]
public class StoreBasedUserProvider(IUserStore userStore) : IUserProvider
{
    private readonly IUserStore _userStore = userStore;

    public async Task<User?> FindAsync(UserFilter filter, CancellationToken cancellationToken = default)
    {
        return await _userStore.FindAsync(filter, cancellationToken);
    }
}