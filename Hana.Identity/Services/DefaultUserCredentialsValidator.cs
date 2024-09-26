using Hana.Extensions;
using Hana.Identity.Contracts;
using Hana.Identity.Entities;

namespace Hana.Identity.Services;


public class DefaultUserCredentialsValidator(IUserProvider userProvider, ISecretHasher secretHasher) : IUserCredentialsValidator
{
    private readonly IUserProvider _userProvider = userProvider;
    private readonly ISecretHasher _secretHasher = secretHasher;

    public async ValueTask<User?> ValidateAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userProvider.FindByNameAsync(username, cancellationToken);

        if (user == null)
            return default;

        var isValidPassword = _secretHasher.VerifySecret(password, user.HashedPassword, user.HashedPasswordSalt);

        return isValidPassword ? user : default;
    }
}