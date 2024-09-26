using Hana.Identity.Contracts;
using Hana.Identity.Entities;
using Hana.Identity.Models;

namespace Hana.Identity.Providers;

public class StoreBasedApplicationProvider(IApplicationStore applicationStore) : IApplicationProvider
{
    private readonly IApplicationStore _applicationStore = applicationStore;

    public async Task<Application?> FindAsync(ApplicationFilter filter, CancellationToken cancellationToken = default)
    {
        return await _applicationStore.FindAsync(filter, cancellationToken);
    }
}