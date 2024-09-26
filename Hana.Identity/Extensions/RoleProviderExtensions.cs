using Hana.Identity.Contracts;
using Hana.Identity.Entities;
using Hana.Identity.Models;

// ReSharper disable once CheckNamespace
namespace Hana.Extensions;

public static class RoleProviderExtensions
{
    public static async Task<IEnumerable<Role>> FindByIdsAsync(this IRoleProvider roleProvider, IEnumerable<string> ids, CancellationToken cancellationToken = default)
    {
        var filter = new RoleFilter()
        {
            Ids = ids.Distinct().ToList()
        };

        return await roleProvider.FindManyAsync(filter, cancellationToken);
    }
}