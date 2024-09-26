using Hana.Identity.Contracts;
using Hana.Identity.Entities;
using Hana.Identity.Models;

// ReSharper disable once CheckNamespace
namespace Hana.Extensions;

public static class UserProviderExtensions
{
    public static async Task<User?> FindByNameAsync(this IUserProvider userProvider, string name, CancellationToken cancellationToken = default)
    {
        var filter = new UserFilter()
        {
            Name = name
        };

        return await userProvider.FindAsync(filter, cancellationToken);
    }
}