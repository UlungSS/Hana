using Hana.Common.Contexts;
using Hana.Common.Contracts;
using Hana.Common.Results;

namespace Hana.Common.Abstractions;


public abstract class TenantResolutionStrategyBase : ITenantResolutionStrategy
{
    ValueTask<TenantResolutionResult> ITenantResolutionStrategy.ResolveAsync(TenantResolutionContext context)
    {
        return ResolveAsync(context);
    }

    protected virtual ValueTask<TenantResolutionResult> ResolveAsync(TenantResolutionContext context)
    {
        return new(Resolve(context));
    }

    protected virtual TenantResolutionResult Resolve(TenantResolutionContext context)
    {
        return Unresolved();
    }
    
    protected static TenantResolutionResult Resolved(string tenantId) => new(tenantId);
    
    protected static TenantResolutionResult Unresolved() => new(null);
    
    protected TenantResolutionResult AutoResolve(string? tenantId) => tenantId == null ? Unresolved() : Resolved(tenantId);
}