using JetBrains.Annotations;

namespace Hana.Common.Entities;


[UsedImplicitly]
public class Tenant : Entity
{
    public string Name { get; set; } = default!;
}
