using System.Security.Claims;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

using Hana.Extensions;

namespace Hana.Requirements;

public class LocalHostPermissionRequirement : IAuthorizationRequirement
{
}

[PublicAPI]
public class LocalHostPermissionRequirementHandler(IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<LocalHostPermissionRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LocalHostPermissionRequirement requirement)
    {
        if (_httpContextAccessor.HttpContext?.Request.IsLocal() == false)
            return Task.CompletedTask;
        
        var currentIdentity = context.User.Identity;

        if (currentIdentity?.IsAuthenticated == false)
        {
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim("permissions", "create:application"));
            identity.AddClaim(new Claim("permissions", "create:user"));
            identity.AddClaim(new Claim("permissions", "create:role"));
            context.User.AddIdentity(identity);
        }

        
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}