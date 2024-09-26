using Hana.Identity.Entities;

namespace Hana.Identity.Options;

public class UsersOptions
{
    public ICollection<User> Users { get; set; } = [];
}