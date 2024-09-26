using Hana.Features.Abstractions;
using Hana.Features.Attributes;
using Hana.Features.Services;

namespace Hana.Features;


public class AppFeature(IModule module) : FeatureBase(module)
{

    public Action<IModule>? Configurator { get; set; }

    public override void Configure()
    {
        Configurator?.Invoke(Module);
    }
}