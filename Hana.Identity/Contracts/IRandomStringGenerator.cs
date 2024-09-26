namespace Hana.Identity.Contracts;

public interface IRandomStringGenerator
{
    string Generate(int length = 32, char[]? chars = null);
}