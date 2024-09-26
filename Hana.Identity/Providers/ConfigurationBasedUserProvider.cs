using Hana.Identity.Contracts;
using Hana.Identity.Entities;
using Hana.Identity.Models;
using Hana.Identity.Options;

using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Hana.Identity.Providers;


[PublicAPI]
public class ConfigurationBasedUserProvider(IOptions<UsersOptions> options) : IUserProvider
{
    private readonly IOptions<UsersOptions> _options = options;

    public Task<User?> FindAsync(UserFilter filter, CancellationToken cancellationToken = default)
    {
        var usersQueryable = _options.Value.Users.AsQueryable();
        var user = filter.Apply(usersQueryable).FirstOrDefault();
        return Task.FromResult(user);
    }
}