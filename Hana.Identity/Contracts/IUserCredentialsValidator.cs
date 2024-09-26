using Hana.Identity.Entities;

namespace Hana.Identity.Contracts;

public interface IUserCredentialsValidator
{
    ValueTask<User?> ValidateAsync(string username, string password, CancellationToken cancellationToken = default);
}