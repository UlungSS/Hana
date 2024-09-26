using Hana.Common.Entities;

namespace Hana.Identity.Entities;

public class Application : Entity
{
    public string ClientId { get; set; } = default!;

    public string HashedClientSecret { get; set; } = default!;

    public string HashedClientSecretSalt { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string HashedApiKey { get; set; } = default!;

    public string HashedApiKeySalt { get; set; } = default!;

    public ICollection<string> Roles { get; set; } = [];
}