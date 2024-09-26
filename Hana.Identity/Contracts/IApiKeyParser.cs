namespace Hana.Identity.Contracts;

public interface IApiKeyParser
{
    string Parse(string apiKey);
}