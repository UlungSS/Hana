using Hana.Identity.Entities;

namespace Hana.Identity.Contracts;

public interface IApplicationCredentialsValidator
{
    ValueTask<Application?> ValidateAsync(string apiKey, CancellationToken cancellationToken = default);
}