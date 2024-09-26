using Hana.Common.Contexts;
using Hana.Common.Results;

namespace Hana.Common.Contracts;

public interface ITenantResolutionStrategy
{
    ValueTask<TenantResolutionResult> ResolveAsync(TenantResolutionContext context);
}