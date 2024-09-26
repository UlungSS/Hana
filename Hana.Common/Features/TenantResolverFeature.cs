using Hana.Common.Contracts;
using Hana.Common.Services;
using Hana.Features.Abstractions;
using Hana.Features.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Hana.Common.Features;

public class TenantResolverFeature(IModule module) : FeatureBase(module)
{
    public Func<IServiceProvider, ITenantResolver> TenantResolver { get; set; } = sp => sp.GetRequiredService<DefaultTenantResolver>();
    
    public override void Apply()
    {
        Services.AddTransient<DefaultTenantResolver>();
        Services.AddScoped(TenantResolver);
    }
}