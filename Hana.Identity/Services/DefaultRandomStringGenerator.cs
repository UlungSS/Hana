using System.Text;
using Hana.Identity.Constants;
using Hana.Identity.Contracts;

namespace Hana.Identity.Services;


public class DefaultRandomStringGenerator : IRandomStringGenerator
{
    private readonly Random _random;

    public DefaultRandomStringGenerator()
    {
        _random = new Random();
    }

    public string Generate(int length = 32, char[]? chars = null)
    {
        var identifierBuilder = new StringBuilder(length);

        chars ??= CharacterSequences.AlphanumericSequence;

        for (var i = 0; i < length; i++)
        {
            var randomIndex = _random.Next(chars.Length);
            identifierBuilder.Append(chars[randomIndex]);
        }

        return identifierBuilder.ToString();
    }
}