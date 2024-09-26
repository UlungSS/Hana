using Hana.Common.Abstractions;
using Hana.Common.Contexts;
using Hana.Common.Results;
using Hana.Identity.Contracts;
using Hana.Identity.Models;

using Microsoft.AspNetCore.Http;

namespace Hana.Identity.MultiTenancy;

public class CurrentUserTenantResolver(IUserProvider userProvider, IHttpContextAccessor httpContextAccessor) : TenantResolutionStrategyBase
{
    protected override async ValueTask<TenantResolutionResult> ResolveAsync(TenantResolutionContext context)
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext == null)
            return Unresolved();

        var userName = httpContext.User.Identity?.Name;

        if (userName == null)
            return Unresolved();

        var cancellationToken = context.CancellationToken;
        var filter = new UserFilter
        {
            Name = userName
        };
        var user = await userProvider.FindAsync(filter, cancellationToken);
        var tenantId = user?.TenantId;

        return AutoResolve(tenantId);
    }
}