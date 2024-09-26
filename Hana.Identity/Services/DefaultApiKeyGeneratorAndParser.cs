using System.Text;
using Hana.Identity.Contracts;

namespace Hana.Identity.Services;


public class DefaultApiKeyGeneratorAndParser : IApiKeyGenerator, IApiKeyParser
{
    public string Generate(string clientId)
    {
        var hexIdentifier = Convert.ToHexString(Encoding.UTF8.GetBytes(clientId));
        var id = Guid.NewGuid().ToString("D");
        return $"{hexIdentifier}-{id}";
    }

    public string Parse(string apiKey)
    {
        var firstSeparatorIndex = apiKey.IndexOf('-');
        var hexIdentifier = apiKey[..firstSeparatorIndex];
        var clientId = Encoding.UTF8.GetString(Convert.FromHexString(hexIdentifier));

        return clientId;
    }
}