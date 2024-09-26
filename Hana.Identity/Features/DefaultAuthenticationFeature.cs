using AspNetCore.Authentication.ApiKey;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

using Hana.Extensions;
using Hana.Features.Abstractions;
using Hana.Features.Attributes;
using Hana.Features.Services;
using Hana.Identity.Providers;
using Hana.Requirements;



namespace Hana.Identity.Features;


[DependsOn(typeof(IdentityFeature))]
public class DefaultAuthenticationFeature(IModule module) : FeatureBase(module)
{
    private const string MultiScheme = "Jwt-or-ApiKey";
    private Func<AuthenticationBuilder, AuthenticationBuilder> _configureApiKeyAuthorization = builder => builder.AddApiKeyInAuthorizationHeader<DefaultApiKeyProvider>();

    public Type ApiKeyProviderType { get; set; } = typeof(DefaultApiKeyProvider);

    public DefaultAuthenticationFeature UseApiKeyAuthorization<T>() where T : class, IApiKeyProvider
    {
        _configureApiKeyAuthorization = builder => builder.AddApiKeyInAuthorizationHeader<T>();
        return this;
    }

    public DefaultAuthenticationFeature UseAdminApiKey() => UseApiKeyAuthorization<AdminApiKeyProvider>();

    public override void Apply()
    {
        Services.ConfigureOptions<ConfigureJwtBearerOptions>();
        Services.ConfigureOptions<ValidateIdentityTokenOptions>();

        var authBuilder = Services
            .AddAuthentication(MultiScheme)
            .AddPolicyScheme(MultiScheme, MultiScheme, options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    return context.Request.Headers.Authorization.Any(x => x!.Contains(ApiKeyDefaults.AuthenticationScheme))
                        ? ApiKeyDefaults.AuthenticationScheme
                        : JwtBearerDefaults.AuthenticationScheme;
                };
            })
            .AddJwtBearer();

        _configureApiKeyAuthorization(authBuilder);

        Services.AddScoped<IAuthorizationHandler, LocalHostRequirementHandler>();
        Services.AddScoped<IAuthorizationHandler, LocalHostPermissionRequirementHandler>();
        Services.AddScoped(ApiKeyProviderType);
        Services.AddScoped<IApiKeyProvider>(sp => (IApiKeyProvider)sp.GetRequiredService(ApiKeyProviderType));
#pragma warning disable ASP0025 // Use AddAuthorizationBuilder
        Services.AddAuthorization(options => options.AddPolicy(IdentityPolicyNames.SecurityRoot, policy => policy.AddRequirements(new LocalHostPermissionRequirement())));
#pragma warning restore ASP0025 // Use AddAuthorizationBuilder
    }
}