using Hana.Identity.Contracts;
using FastEndpoints;
using JetBrains.Annotations;

namespace Hana.Identity.Endpoints.Secrets.Hash;


[PublicAPI]
internal class Hash(ISecretHasher secretHasher) : Endpoint<Request, Response>
{
    private readonly ISecretHasher _secretHasher = secretHasher;

    public override void Configure()
    {
        Post("/identity/secrets/hash");
        Policies(IdentityPolicyNames.SecurityRoot);
    }

    public override Task<Response> ExecuteAsync(Request request, CancellationToken cancellationToken)
    {
        var hashedPassword = _secretHasher.HashSecret(request.Secret);
        var response = new Response(hashedPassword.EncodeSecret(), hashedPassword.EncodeSalt());

        return Task.FromResult(response);
    }
}