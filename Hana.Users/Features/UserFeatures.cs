using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

using Hana.Features.Abstractions;
using Hana.Features.Services;

using Hana.Users.Contract;
using Hana.Users.Repositories;
using Hana.Users.Services;
using Hana.Extensions;

namespace Hana.Users.Features;

[PublicAPI]
public class UserFeatures(IModule module) : FeatureBase(module)
{
    public override void Configure()
    {
        Module.AddFastEndpointsAssembly(GetType());
    }
    public override void Apply()
    {
        Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        Services.AddScoped<IUserRepository, UserRepository>();
        Services.AddScoped<IUserServices, UserServices>();
    }
}
