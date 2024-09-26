namespace Hana.Identity.Contracts;

public interface IApiKeyGenerator
{
    string Generate(string clientId);
}