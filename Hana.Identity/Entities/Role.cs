using Hana.Common.Entities;

namespace Hana.Identity.Entities;

public class Role : Entity
{
    public string Name { get; set; } = default!;

    public ICollection<string> Permissions { get; set; } = [];
}