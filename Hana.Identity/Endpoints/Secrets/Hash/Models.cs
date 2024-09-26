using System.ComponentModel.DataAnnotations;

namespace Hana.Identity.Endpoints.Secrets.Hash;

internal class Request
{
    [Required] public string Secret { get; set; } = default!;
}

internal class Response(string hashedSecret, string salt)
{
    public string HashedSecret { get; set; } = hashedSecret;
    public string Salt { get; set; } = salt;
}