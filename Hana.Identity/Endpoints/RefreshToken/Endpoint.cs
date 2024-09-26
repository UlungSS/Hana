using Hana.Extensions;
using Hana.Identity.Contracts;
using Hana.Identity.Models;

using FastEndpoints;
using JetBrains.Annotations;

namespace Hana.Identity.Endpoints.RefreshToken;


[PublicAPI]
internal class RefreshToken(IUserProvider userProvider, IAccessTokenIssuer tokenIssuer) : EndpointWithoutRequest<LoginResponse>
{
    private readonly IUserProvider _userProvider = userProvider;
    private readonly IAccessTokenIssuer _tokenIssuer = tokenIssuer;

    public override void Configure()
    {
        Post("/identity/refresh-token");
    }

    public override async Task<LoginResponse> ExecuteAsync(CancellationToken cancellationToken)
    {
        var user = await _userProvider.FindByNameAsync(User.Identity!.Name!, cancellationToken);

        if (user == null)
            return new LoginResponse(false, null, null);

        var tokens = await _tokenIssuer.IssueTokensAsync(user, cancellationToken);

        return new LoginResponse(true, tokens.AccessToken, tokens.RefreshToken);
    }
}