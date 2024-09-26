using Hana.Identity.Contracts;
using Hana.Identity.Entities;
using Hana.Identity.Models;

namespace Hana.Identity.Providers;


public class AdminUserProvider : IUserProvider
{
    private readonly User _adminUser;

    public AdminUserProvider(ISecretHasher secretHasher)
    {
        var hashedSecret = secretHasher.HashSecret("password");

        _adminUser = new User
        {
            Id = "admin",
            Name = "admin",
            HashedPassword = hashedSecret.EncodeSecret(),
            HashedPasswordSalt = hashedSecret.EncodeSalt(),
            Roles = { "admin" }
        };
    }

    public Task<User?> FindAsync(UserFilter filter, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_adminUser)!;
    }
}