using Hana.Abstractions;
using Hana.Identity.Contracts;
using Hana.Identity.Entities;
using Hana.Core.Contracts;

using JetBrains.Annotations;

namespace Hana.Identity.Endpoints.Users.Create;


[PublicAPI]
internal class Create(
    IIdentityGenerator identityGenerator,
    ISecretGenerator secretGenerator,
    ISecretHasher secretHasher,
    IUserStore userStore,
    IRoleStore roleStore) : HanaEndpoint<Request, Response>
{
    private readonly IIdentityGenerator _identityGenerator = identityGenerator;
    private readonly ISecretGenerator _secretGenerator = secretGenerator;
    private readonly ISecretHasher _secretHasher = secretHasher;
    private readonly IUserStore _userStore = userStore;
    private readonly IRoleStore _roleStore = roleStore;


    public override void Configure()
    {
        Post("/identity/users");
        ConfigurePermissions("create:user");
        Policies(IdentityPolicyNames.SecurityRoot);
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        var id = _identityGenerator.GenerateId();
        var password = string.IsNullOrWhiteSpace(request.Password) ? _secretGenerator.Generate() : request.Password.Trim();
        var hashedPassword = _secretHasher.HashSecret(password);

        var user = new User
        {
            Id = id,
            Name = request.Name,
            Roles = request.Roles ?? [],
            HashedPassword = hashedPassword.EncodeSecret(),
            HashedPasswordSalt = hashedPassword.EncodeSalt()
        };

        await _userStore.SaveAsync(user, cancellationToken);

        var response = new Response(
            id,
            user.Name,
            password,
            user.Roles,
            user.TenantId,
            hashedPassword.EncodeSecret(),
            hashedPassword.EncodeSalt());

        await SendOkAsync(response, cancellationToken);
    }
}