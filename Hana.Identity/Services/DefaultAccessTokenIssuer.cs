using System.Security.Claims;
using FastEndpoints.Security;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;

using Hana.Common.Contracts;
using Hana.Extensions;
using Hana.Identity.Contracts;
using Hana.Identity.Entities;
using Hana.Identity.Models;
using Hana.Identity.Options;


namespace Hana.Identity.Services;

public class DefaultAccessTokenIssuer(IRoleProvider roleProvider, ISystemClock systemClock, IOptions<IdentityTokenOptions> identityTokenOptions) : IAccessTokenIssuer
{
    private readonly IRoleProvider _roleProvider = roleProvider;
    private readonly ISystemClock _systemClock = systemClock;
    private readonly IdentityTokenOptions _identityOptions = identityTokenOptions.Value;

    [Obsolete]
    public async ValueTask<IssuedTokens> IssueTokensAsync(User user, CancellationToken cancellationToken = default)
    {
        var roles = (await _roleProvider.FindByIdsAsync(user.Roles, cancellationToken)).ToList();
        var permissions = roles.SelectMany(x => x.Permissions).ToList();
        var signingKey = _identityOptions.SigningKey;
        var issuer = _identityOptions.Issuer;
        var audience = _identityOptions.Audience;
        var accessTokenLifetime = _identityOptions.AccessTokenLifetime;
        var refreshTokenLifetime = _identityOptions.RefreshTokenLifetime;

        if (string.IsNullOrWhiteSpace(signingKey)) throw new Exception("No signing key configured");
        if (string.IsNullOrWhiteSpace(issuer)) throw new Exception("No issuer configured");
        if (string.IsNullOrWhiteSpace(audience)) throw new Exception("No audience configured");

        var nameClaim = new Claim(JwtRegisteredClaimNames.Name, user.Name);
        var claims = new List<Claim> { nameClaim };
        
        if (!string.IsNullOrWhiteSpace(user.TenantId))
        {
            var tenantIdClaim = new Claim(_identityOptions.TenantIdClaimsType, user.TenantId);
            claims.Add(tenantIdClaim);
        }

        var accessTokenExpiresAt = _systemClock.UtcNow.Add(accessTokenLifetime);
        var refreshTokenExpiresAt = _systemClock.UtcNow.Add(refreshTokenLifetime);
        var accessToken = JWTBearer.CreateToken(signingKey, accessTokenExpiresAt.UtcDateTime, permissions, issuer: issuer, audience: audience, claims: claims);
        var refreshToken = JWTBearer.CreateToken(signingKey, refreshTokenExpiresAt.UtcDateTime, permissions, issuer: issuer, audience: audience, claims: claims);

        return new IssuedTokens(accessToken, refreshToken);
    }
}