using Hana.Common.Abstractions;
using Hana.Common.Contexts;
using Hana.Common.Results;
using Hana.Identity.Options;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Hana.Identity.MultiTenancy;

public class ClaimsTenantResolver(IHttpContextAccessor httpContextAccessor, IOptions<IdentityTokenOptions> options) : TenantResolutionStrategyBase
{
    protected override TenantResolutionResult Resolve(TenantResolutionContext context)
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext == null)
            return Unresolved();

        var tenantId = httpContext.User.FindFirst(options.Value.TenantIdClaimsType)?.Value;
        return AutoResolve(tenantId);
    }
}