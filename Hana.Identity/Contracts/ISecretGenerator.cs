namespace Hana.Identity.Contracts;

public interface ISecretGenerator
{
    string Generate(int length = 32);
}