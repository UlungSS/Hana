using Hana.Extensions;
using Hana.Features.Abstractions;
using Hana.Features.Services;

using Hana.Core.Api.Constants;
using Hana.Core.Api.Requirements;
using Hana.Core.Api.Serialization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Hana.Core.Api.Features;


public class CoreApiFeature(IModule module) : FeatureBase(module)
{

    public override void Configure()
    {
        Module.AddFastEndpointsAssembly(GetType());
    }

    public override void Apply()
    {
        Services.AddSerializationOptionsConfigurator<SerializationConfigurator>();
        Module.AddFastEndpointsFromModule();


        Services.AddScoped<IAuthorizationHandler, NotReadOnlyRequirementHandler>();
        Services.Configure<AuthorizationOptions>(options =>
        {
            options.AddPolicy(AuthorizationPolicies.NotReadOnlyPolicy, policy => policy.AddRequirements(new NotReadOnlyRequirement()));
        });
    }
}