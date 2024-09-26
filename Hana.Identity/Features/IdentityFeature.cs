using AspNetCore.Authentication.ApiKey;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

using Hana.Common.Contracts;
using Hana.Common.Features;
using Hana.Extensions;
using Hana.Features.Abstractions;
using Hana.Features.Attributes;
using Hana.Features.Services;
using Hana.Identity.Contracts;
using Hana.Identity.Entities;
using Hana.Identity.MultiTenancy;
using Hana.Identity.Options;
using Hana.Identity.Providers;
using Hana.Identity.Services;


namespace Hana.Identity.Features;


[DependsOn(typeof(SystemClockFeature))]
[PublicAPI]
public class IdentityFeature(IModule module) : FeatureBase(module)
{

    public Action<IdentityTokenOptions> TokenOptions { get; set; } = _ => { };

    public Action<ApiKeyOptions> ApiKeyOptions { get; set; } = options =>
    {
        options.Realm = "Hana Apps";
        options.KeyName = "ApiKey";
    };
    public Action<UsersOptions> UsersOptions { get; set; } = _ => { };

    public Action<ApplicationsOptions> ApplicationsOptions { get; set; } = _ => { };

    public Action<RolesOptions> RolesOptions { get; set; } = _ => { };

    public Func<IServiceProvider, IUserStore> UserStore { get; set; } = sp => sp.GetRequiredService<MemoryUserStore>();

    public Func<IServiceProvider, IApplicationStore> ApplicationStore { get; set; } = sp => sp.GetRequiredService<MemoryApplicationStore>();

    public Func<IServiceProvider, IRoleStore> RoleStore { get; set; } = sp => sp.GetRequiredService<MemoryRoleStore>();

    public Func<IServiceProvider, IUserProvider> UserProvider { get; set; } = sp => sp.GetRequiredService<StoreBasedUserProvider>();

    public Func<IServiceProvider, IApplicationProvider> ApplicationProvider { get; set; } = sp => sp.GetRequiredService<StoreBasedApplicationProvider>();

    public Func<IServiceProvider, IRoleProvider> RoleProvider { get; set; } = sp => sp.GetRequiredService<StoreBasedRoleProvider>();

    public void UseStoreBasedUserProvider() => UserProvider = sp => sp.GetRequiredService<StoreBasedUserProvider>();

    public void UseConfigurationBasedUserProvider(Action<UsersOptions> configure)
    {
        UserProvider = sp => sp.GetRequiredService<ConfigurationBasedUserProvider>();
        UsersOptions += configure;
    }

    public void UseAdminUserProvider()
    {
        UserProvider = sp => sp.GetRequiredService<AdminUserProvider>();
        RoleProvider = sp => sp.GetRequiredService<AdminRoleProvider>();
    }


    public void UseStoreBasedApplicationProvider() => ApplicationProvider = sp => sp.GetRequiredService<StoreBasedApplicationProvider>();

    public void UseConfigurationBasedApplicationProvider(Action<ApplicationsOptions> configure)
    {
        ApplicationProvider = sp => sp.GetRequiredService<ConfigurationBasedApplicationProvider>();
        ApplicationsOptions += configure;
    }

    public void UseStoreBasedRoleProvider() => RoleProvider = sp => sp.GetRequiredService<StoreBasedRoleProvider>();


    public void UseConfigurationBasedRoleProvider(Action<RolesOptions> configure)
    {
        RoleProvider = sp => sp.GetRequiredService<ConfigurationBasedRoleProvider>();
        RolesOptions += configure;
    }

    public override void Configure()
    {
        Module.AddFastEndpointsAssembly(GetType());
    }

    public override void Apply()
    {
        Services.Configure(TokenOptions);
        Services.Configure(ApiKeyDefaults.AuthenticationScheme, ApiKeyOptions);
        Services.Configure(UsersOptions);
        Services.Configure(ApplicationsOptions);
        Services.Configure(RolesOptions);

        // Memory stores.
        Services
            .AddMemoryStore<User, MemoryUserStore>()
            .AddMemoryStore<Application, MemoryApplicationStore>()
            .AddMemoryStore<Role, MemoryRoleStore>();

        // User providers.
        Services
            .AddScoped<AdminUserProvider>()
            .AddScoped<StoreBasedUserProvider>()
            .AddScoped<ConfigurationBasedUserProvider>();

        // Application providers.
        Services
            .AddScoped<StoreBasedApplicationProvider>()
            .AddScoped<ConfigurationBasedApplicationProvider>();

        // Role providers.
        Services
            .AddScoped<AdminRoleProvider>()
            .AddScoped<StoreBasedRoleProvider>()
            .AddScoped<ConfigurationBasedRoleProvider>();

        // Tenant resolution strategies.
        Services
            .AddScoped<ITenantResolutionStrategy, ClaimsTenantResolver>()
            .AddScoped<ITenantResolutionStrategy, CurrentUserTenantResolver>();

        // Services.
        Services
            .AddScoped(UserStore)
            .AddScoped(ApplicationStore)
            .AddScoped(RoleStore)
            .AddScoped(UserProvider)
            .AddScoped(ApplicationProvider)
            .AddScoped(RoleProvider)
            .AddScoped<ISecretHasher, DefaultSecretHasher>()
            .AddScoped<IAccessTokenIssuer, DefaultAccessTokenIssuer>()
            .AddScoped<IUserCredentialsValidator, DefaultUserCredentialsValidator>()
            .AddScoped<IApplicationCredentialsValidator, DefaultApplicationCredentialsValidator>()
            .AddScoped<IApiKeyGenerator>(sp => sp.GetRequiredService<DefaultApiKeyGeneratorAndParser>())
            .AddScoped<IApiKeyParser>(sp => sp.GetRequiredService<DefaultApiKeyGeneratorAndParser>())
            .AddScoped<IClientIdGenerator, DefaultClientIdGenerator>()
            .AddScoped<ISecretGenerator, DefaultSecretGenerator>()
            .AddScoped<IRandomStringGenerator, DefaultRandomStringGenerator>()
            .AddScoped<DefaultApiKeyGeneratorAndParser>()
            ;
    }
}