using Hana.Features.Services;
using Hana.Identity.Features;

// ReSharper disable once CheckNamespace
namespace Hana.Extensions;

public static class ModuleExtensions
{
    public static IModule UseIdentity(this IModule module, Action<IdentityFeature> configure)
    {
        module.Configure(configure);
        return module;
    }

    public static IModule UseIdentity(this IModule module, string signingKey, string issuer = "http://hana.api", string audience = "http://hana.api", TimeSpan? tokenLifetime = default)
    {
        module.UseIdentity(identity => identity.TokenOptions += options =>
        {
            options.Audience = audience;
            options.Issuer = issuer;
            options.AccessTokenLifetime = tokenLifetime ?? TimeSpan.FromHours(1);
            options.SigningKey = signingKey;
        });
        return module;
    }

    public static IModule UseDefaultAuthentication(this IModule module, Action<DefaultAuthenticationFeature>? configure = default)
    {
        module.Configure(configure);
        return module;
    }
}