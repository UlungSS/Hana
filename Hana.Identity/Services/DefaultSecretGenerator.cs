using Hana.Identity.Constants;
using Hana.Identity.Contracts;

namespace Hana.Identity.Services;

public class DefaultSecretGenerator(IRandomStringGenerator randomStringGenerator) : ISecretGenerator
{
    private readonly IRandomStringGenerator _randomStringGenerator = randomStringGenerator;

    public string Generate(int length = 32) => _randomStringGenerator.Generate(length, CharacterSequences.AlphanumericAndSymbolSequence);
}