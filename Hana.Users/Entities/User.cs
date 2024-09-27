using System.Text.Json.Serialization;

namespace Hana.Users.Entities;

public class User
{
    public int Id { get; set; }
    public string? Guid { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public Role Role { get; set; }

    [JsonIgnore]
    public string? Pass_Hash { get; set; }
}
