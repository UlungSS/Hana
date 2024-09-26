using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using Hana.Identity.Constants;

namespace Hana.Identity.Options;

public class IdentityTokenOptions
{
    public string SigningKey { get; set; } = default!;

    public string Issuer { get; set; } = "http://hana.api";

    public string Audience { get; set; } = "http://hana.api";

    public TimeSpan AccessTokenLifetime { get; set; } = TimeSpan.FromHours(1);

    public TimeSpan RefreshTokenLifetime { get; set; } = TimeSpan.FromHours(2);

    public string TenantIdClaimsType { get; set; } = CustomClaimTypes.TenantId;

    public SecurityKey CreateSecurityKey() => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SigningKey));

    public void ConfigureJwtBearerOptions(JwtBearerOptions options)
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = CreateSecurityKey(),
            ValidAudience = Audience,
            ValidIssuer = Issuer,
            ValidateLifetime = true,
            LifetimeValidator = ValidateLifetime,
            NameClaimType = JwtRegisteredClaimNames.Name
        };
    }

    private static bool ValidateLifetime(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
    {
        return expires != null && expires > DateTime.UtcNow;
    }
}