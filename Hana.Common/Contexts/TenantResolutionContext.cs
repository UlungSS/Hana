using Hana.Common.Entities;

namespace Hana.Common.Contexts;

public class TenantResolutionContext(IDictionary<string, Tenant> tenants, CancellationToken cancellationToken)
{
    private readonly IDictionary<string, Tenant> _tenantsDictionary = tenants;

    public CancellationToken CancellationToken { get; } = cancellationToken;

    public Tenant? FindTenant(string tenantId)
    {
        return _tenantsDictionary.TryGetValue(tenantId, out var tenant) ? tenant : null;
    }

    public Tenant? FindTenant(Func<Tenant, bool> predicate)
    {
        return _tenantsDictionary.Values.FirstOrDefault(predicate);
    }
}