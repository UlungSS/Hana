namespace Hana.Common.Entities;

public abstract class Entity
{
    public string Id { get; set; } = default!;

    public string? TenantId { get; set; }
}