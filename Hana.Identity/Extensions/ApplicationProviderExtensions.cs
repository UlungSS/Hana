using Hana.Identity.Contracts;
using Hana.Identity.Entities;
using Hana.Identity.Models;

// ReSharper disable once CheckNamespace
namespace Hana.Extensions;

public static class ApplicationProviderExtensions
{
    public static async Task<Application?> FindByClientIdAsync(this IApplicationProvider applicationProvider, string clientId, CancellationToken cancellationToken = default)
    {
        var filter = new ApplicationFilter
        {
            ClientId = clientId
        };

        return await applicationProvider.FindAsync(filter, cancellationToken);
    }
}