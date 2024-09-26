using Hana.Common.Entities;

namespace Hana.Identity.Entities;

public class User : Entity
{
    public string Name { get; set; } = default!;

    public string HashedPassword { get; set; } = default!;

    public string HashedPasswordSalt { get; set; } = default!;

    public ICollection<string> Roles { get; set; } = [];
}