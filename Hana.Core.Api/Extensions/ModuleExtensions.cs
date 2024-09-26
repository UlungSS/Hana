using Hana.Features.Services;
using Hana.Core.Api.Features;

// ReSharper disable once CheckNamespace
namespace Hana.Extensions;


public static class ModuleExtensions
{
    public static IModule UseHanaCoreApi(this IModule module, Action<CoreApiFeature>? configure = default)
    {
        module.Configure(configure);
        return module;
    }

}