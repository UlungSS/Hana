using Hana.Expressions.Contracts;
using Hana.Expressions.Services;
using Hana.Features.Abstractions;
using Hana.Features.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Hana.Expressions.Features;

public class ExpressionsFeature(IModule module) : FeatureBase(module)
{
    public override void Apply()
    {
        Services
            //.AddScoped<IExpressionEvaluator, ExpressionEvaluator>()
            .AddSingleton<IWellKnownTypeRegistry, WellKnownTypeRegistry>();
    }
}