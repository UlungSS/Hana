using Hana.Abstractions;
using Hana.Identity.Contracts;
using Hana.Identity.Entities;
using Hana.Core.Contracts;

using Humanizer;
using JetBrains.Annotations;

namespace Hana.Identity.Endpoints.Roles.Create;

/// <summary>
/// An endpoint that creates a new role.
/// </summary>
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
        Post("/identity/roles");
        ConfigurePermissions("create:role");
        Policies(IdentityPolicyNames.SecurityRoot);
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        var id = request.Id ?? request.Name.Kebaberize();

        var role = new Role
        {
            Id = id,
            Name = request.Name,
            Permissions = request.Permissions ?? []
        };

        await _roleStore.SaveAsync(role, cancellationToken);

        var response = new Response(
            id,
            role.Name,
            role.Permissions);

        await SendOkAsync(response, cancellationToken);
    }
}