using System.Security.Claims;
using AspNetCore.Authentication.ApiKey;

using Hana.Identity.Contracts;
using Hana.Identity.Models;

namespace Hana.Identity.Providers;

public class DefaultApiKeyProvider(IApplicationCredentialsValidator applicationCredentialsValidator, IRoleProvider roleProvider) : IApiKeyProvider
{
    private readonly IApplicationCredentialsValidator _applicationCredentialsValidator = applicationCredentialsValidator;
    private readonly IRoleProvider _roleProvider = roleProvider;

    public async Task<IApiKey?> ProvideAsync(string key)
    {
        var application = await _applicationCredentialsValidator.ValidateAsync(key);

        if (application == null)
            return null;

        var filter = new RoleFilter { Ids = application.Roles.Distinct().ToList() };
        var roles = (await _roleProvider.FindManyAsync(filter)).ToList();
        var permissions = roles.SelectMany(x => x.Permissions).Distinct().ToList();
        var claims = new List<Claim>();

        claims.AddRange(permissions.Select(p => new Claim("permissions", p)));

        return new ApiKey(key, application.ClientId, claims);
    }
}