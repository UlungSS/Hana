using Hana.Identity.Entities;

namespace Hana.Identity.Options;

public class RolesOptions
{
    public ICollection<Role> Roles { get; set; } = [];
}