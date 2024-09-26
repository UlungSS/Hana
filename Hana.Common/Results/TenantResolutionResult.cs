namespace Hana.Common.Results;

public record TenantResolutionResult(string? TenantId)
{
    public static TenantResolutionResult Resolved(string tenantId) => new(tenantId);

    public static TenantResolutionResult Unresolved() => new(default(string?));

    public bool IsResolved => TenantId != null;
}