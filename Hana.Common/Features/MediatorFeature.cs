using Hana.Features.Abstractions;
using Hana.Features.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Hana.Common.Features;

public class MediatorFeature(IModule module) : FeatureBase(module)
{

    public override void Apply()
    {
        //Services
        //    .AddMediator()
        //    .AddMediatorHostedServices();
    }
}