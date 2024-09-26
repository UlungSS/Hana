using Hana.Common.Contracts;
using Hana.Common.Services;
using Hana.Features.Abstractions;
using Hana.Features.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Hana.Common.Features;

public class DefaultFormattersFeature(IModule module) : FeatureBase(module)
{
    public override void Configure()
    {
        Module.Services.AddSingleton<IFormatter, JsonFormatter>();
    }
}