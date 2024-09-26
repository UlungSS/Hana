using Hana.Abstractions;
using Hana.Identity.Contracts;
using Hana.Identity.Entities;
using Hana.Core.Contracts;

using JetBrains.Annotations;

namespace Hana.Identity.Endpoints.Applications.Create;


[PublicAPI]
internal class Create(
    IIdentityGenerator identityGenerator,
    IClientIdGenerator clientIdGenerator,
    ISecretGenerator secretGenerator,
    IApiKeyGenerator apiKeyGenerator,
    ISecretHasher secretHasher,
    IApplicationStore applicationStore,
    IRoleStore roleStore) : HanaEndpoint<Request, Response>
{
    private readonly IIdentityGenerator _identityGenerator = identityGenerator;
    private readonly IClientIdGenerator _clientIdGenerator = clientIdGenerator;
    private readonly ISecretGenerator _secretGenerator = secretGenerator;
    private readonly IApiKeyGenerator _apiKeyGenerator = apiKeyGenerator;
    private readonly ISecretHasher _secretHasher = secretHasher;
    private readonly IApplicationStore _applicationStore = applicationStore;
    private readonly IRoleStore _roleStore = roleStore;


    public override void Configure()
    {
        Post("/identity/applications");
        ConfigurePermissions("create:application");
        Policies(IdentityPolicyNames.SecurityRoot);
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        var id = _identityGenerator.GenerateId();
        var clientId = await _clientIdGenerator.GenerateAsync(cancellationToken);
        var clientSecret = _secretGenerator.Generate();
        var hashedClientSecret = _secretHasher.HashSecret(clientSecret);
        var apiKey = _apiKeyGenerator.Generate(clientId);
        var hashedApiKey = _secretHasher.HashSecret(apiKey);

        var application = new Application
        {
            Id = id,
            ClientId = clientId,
            HashedClientSecret = hashedClientSecret.EncodeSecret(),
            HashedClientSecretSalt = hashedClientSecret.EncodeSalt(),
            Name = request.Name,
            HashedApiKey = hashedApiKey.EncodeSecret(),
            HashedApiKeySalt = hashedApiKey.EncodeSalt(),
            Roles = request.Roles ?? new List<string>()
        };

        await _applicationStore.SaveAsync(application, cancellationToken);

        var response = new Response(
            id,
            application.Name,
            application.Roles,
            clientId,
            clientSecret,
            apiKey,
            hashedApiKey.EncodeSecret(),
            hashedApiKey.EncodeSalt(),
            hashedClientSecret.EncodeSecret(),
            hashedClientSecret.EncodeSalt());

        await SendOkAsync(response, cancellationToken);
    }
}