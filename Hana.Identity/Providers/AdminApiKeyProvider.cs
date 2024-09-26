using System.Security.Claims;
using AspNetCore.Authentication.ApiKey;

using Hana.Identity.Models;

namespace Hana.Identity.Providers;


public class AdminApiKeyProvider : IApiKeyProvider
{
    public static readonly string DefaultApiKey = Guid.Empty.ToString();

    public Task<IApiKey?> ProvideAsync(string key)
    {
        if (key != DefaultApiKey)
            return Task.FromResult<IApiKey?>(null);

        var claims = new List<Claim> { new("permissions", "*") };
        var apiKey = new ApiKey(key, "admin", claims);
        return Task.FromResult<IApiKey>(apiKey)!;
    }
}