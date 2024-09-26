using Hana.Identity.Entities;
using Hana.Identity.Models;

namespace Hana.Identity.Contracts;

public interface IApplicationStore
{
    Task SaveAsync(Application application, CancellationToken cancellationToken = default);

    Task DeleteAsync(ApplicationFilter filter, CancellationToken cancellationToken = default);

    Task<Application?> FindAsync(ApplicationFilter filter, CancellationToken cancellationToken = default);
}