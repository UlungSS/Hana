using Hana.Common.Entities;

namespace Hana.Common.Contracts;

public interface ITenantResolver
{
    Task<Tenant?> GetTenantAsync(CancellationToken cancellationToken = default);
}