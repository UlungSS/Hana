using Hana.Common.Contracts;
using Hana.Common.Entities;

namespace Hana.Common.Services;

public class DefaultTenantResolver : ITenantResolver
{
    private readonly Tenant _defaultTenant = new()
    {
        Id = null!,
        Name = "Default"
    };

    public Task<Tenant?> GetTenantAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<Tenant?>(_defaultTenant);
    }
}