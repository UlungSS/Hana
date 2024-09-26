using Hana.Identity.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Hana.Extensions;

public class ConfigureJwtBearerOptions(IOptions<IdentityTokenOptions> identityTokenOptions) : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly IOptions<IdentityTokenOptions> _identityTokenOptions = identityTokenOptions;

    public void Configure(JwtBearerOptions options) => Configure(null, options);

    public void Configure(string? name, JwtBearerOptions options)
    {
        _identityTokenOptions.Value.ConfigureJwtBearerOptions(options);
    }
}