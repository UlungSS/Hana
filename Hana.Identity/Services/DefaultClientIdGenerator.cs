using Hana.Identity.Constants;
using Hana.Identity.Contracts;
using Hana.Identity.Models;

namespace Hana.Identity.Services;


public class DefaultClientIdGenerator(IRandomStringGenerator randomStringGenerator, IApplicationProvider applicationProvider) : IClientIdGenerator
{
    private readonly IRandomStringGenerator _randomStringGenerator = randomStringGenerator;
    private readonly IApplicationProvider _applicationProvider = applicationProvider;

    public async Task<string> GenerateAsync(CancellationToken cancellationToken = default)
    {
        while (true)
        {
            var clientId = _randomStringGenerator.Generate(16, CharacterSequences.AlphanumericSequence);
            var filter = new ApplicationFilter { ClientId = clientId };
            var application = await _applicationProvider.FindAsync(filter, cancellationToken);

            if (application == null)
                return clientId;
        }
    }
}