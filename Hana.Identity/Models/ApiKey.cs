using System.Security.Claims;
using AspNetCore.Authentication.ApiKey;

namespace Hana.Identity.Models;

public record ApiKey(string Key, string OwnerName, IReadOnlyCollection<Claim> Claims) : IApiKey;