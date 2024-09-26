using Hana.Common.Contracts;
using Hana.Common.Services;
using Hana.Features.Abstractions;
using Hana.Features.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Hana.Common.Features;

public class SystemClockFeature(IModule module) : FeatureBase(module)
{
    public override void Apply()
    {
        Services.AddSingleton<ISystemClock, SystemClock>();
    }
}