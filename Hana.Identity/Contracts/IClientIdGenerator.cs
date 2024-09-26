namespace Hana.Identity.Contracts;

public interface IClientIdGenerator
{
    Task<string> GenerateAsync(CancellationToken cancellationToken = default);
}