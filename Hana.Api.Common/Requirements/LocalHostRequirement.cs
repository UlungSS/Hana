using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

using Hana.Extensions;

namespace Hana.Requirements;

public class LocalHostRequirement : IAuthorizationRequirement
{
}

[PublicAPI]
public class LocalHostRequirementHandler(IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<LocalHostRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LocalHostRequirement requirement)
    {
        if (_httpContextAccessor.HttpContext?.Request.IsLocal() == false)
            context.Fail(new AuthorizationFailureReason(this, "Only requests from localhost are allowed"));

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}