using Hana.Identity.Entities;
using Hana.Identity.Models;

namespace Hana.Identity.Contracts;

public interface IApplicationProvider
{
    Task<Application?> FindAsync(ApplicationFilter filter, CancellationToken cancellationToken = default);
}