using Hana.Identity.Contracts;
using Hana.Identity.Entities;
using Hana.Identity.Models;
using Hana.Identity.Options;

using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Hana.Identity.Providers;


[PublicAPI]
public class ConfigurationBasedApplicationProvider(IOptions<ApplicationsOptions> options) : IApplicationProvider
{
    private readonly IOptions<ApplicationsOptions> _options = options;

    public Task<Application?> FindAsync(ApplicationFilter filter, CancellationToken cancellationToken = default)
    {
        var applicationsQueryable = _options.Value.Applications.AsQueryable();
        var application = filter.Apply(applicationsQueryable).FirstOrDefault();
        return Task.FromResult(application);
    }
}