using Hana.Extensions;
using Hana.Identity.Contracts;
using Hana.Identity.Entities;

using JetBrains.Annotations;

namespace Hana.Identity.Services;


[PublicAPI]
public class DefaultApplicationCredentialsValidator(IApiKeyParser apiKeyParser, IApplicationProvider applicationProvider, ISecretHasher secretHasher) : IApplicationCredentialsValidator
{
    private readonly IApiKeyParser _apiKeyParser = apiKeyParser;
    private readonly IApplicationProvider _applicationProvider = applicationProvider;
    private readonly ISecretHasher _secretHasher = secretHasher;

      public async ValueTask<Application?> ValidateAsync(string apiKey, CancellationToken cancellationToken = default)
    {
        if(string.IsNullOrWhiteSpace(apiKey))
            return default;
        
        var clientId = _apiKeyParser.Parse(apiKey);
        var application = await _applicationProvider.FindByClientIdAsync(clientId, cancellationToken);
        
        if(application == null)
            return default;
        
        var isValidApiKey = _secretHasher.VerifySecret(apiKey, application.HashedApiKey, application.HashedApiKeySalt);
        return isValidApiKey ? application : default;
    }
}