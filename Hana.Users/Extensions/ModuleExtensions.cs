using Hana.Features.Services;
using Hana.Users.Features;

namespace Hana.Users.Extensions;

public static class ModuleExtensions
{
    public static IModule UseModuleUser(this IModule module, Action<UserFeatures>? configure = null)
    {
        module.Configure(configure);
        return module;
    }
}
