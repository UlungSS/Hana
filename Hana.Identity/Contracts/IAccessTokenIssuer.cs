using Hana.Identity.Entities;
using Hana.Identity.Models;

namespace Hana.Identity.Contracts;

public interface IAccessTokenIssuer
{
    ValueTask<IssuedTokens> IssueTokensAsync(User user, CancellationToken cancellationToken = default);
}