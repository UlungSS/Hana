using Hana.Identity.Contracts;
using Hana.Identity.Models;
using FastEndpoints;
using JetBrains.Annotations;

namespace Hana.Identity.Endpoints.Login;

[PublicAPI]
internal class Login(IUserCredentialsValidator userCredentialsValidator, IAccessTokenIssuer tokenIssuer) : Endpoint<Request, LoginResponse>
{
    private readonly IUserCredentialsValidator _userCredentialsValidator = userCredentialsValidator;
    private readonly IAccessTokenIssuer _tokenIssuer = tokenIssuer;

    public override void Configure()
    {
        Post("/identity/login");
        AllowAnonymous();
    }

    public override async Task<LoginResponse> ExecuteAsync(Request request, CancellationToken cancellationToken)
    {
        var user = await _userCredentialsValidator.ValidateAsync(request.Username.Trim(), request.Password.Trim(), cancellationToken);

        if (user == null)
            return new LoginResponse(false, null, null);

        var tokens = await _tokenIssuer.IssueTokensAsync(user, cancellationToken);

        return new LoginResponse(true, tokens.AccessToken, tokens.RefreshToken);
    }
}