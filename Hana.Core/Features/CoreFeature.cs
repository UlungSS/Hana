using Hana.Common.Contracts;
using Hana.Common.Features;
using Hana.Common.Serialization;
using Hana.Expressions.Features;
using Hana.Extensions;
using Hana.Features.Abstractions;
using Hana.Features.Attributes;
using Hana.Features.Services;

using Hana.Core.Contracts;

using Microsoft.Extensions.DependencyInjection;
using Hana.Core.Serializer;
using Hana.Core.Models;
using Hana.Core.Context;

namespace Hana.Core.Features;

// Adds services to the system.
[DependsOn(typeof(SystemClockFeature))]
[DependsOn(typeof(ExpressionsFeature))]
[DependsOn(typeof(DefaultFormattersFeature))]
[DependsOn(typeof(TenantResolverFeature))]
public class CoreFeature(IModule module) : FeatureBase(module)
{
    public Action<PersistenceSetting> PersistenceOptions { get; set; } = _ => { };
    public Func<IServiceProvider, IIdentityGenerator> IdentityGenerator { get; set; } = sp => new RandomLongIdentityGenerator();

    public CoreFeature WithIdentityGenerator(Func<IServiceProvider, IIdentityGenerator> generator)
    {
        IdentityGenerator = generator;
        return this;
    }


    public override void Apply()
    {
        AddHanaCore(Services);

        //Init database
        var sp = Services.BuildServiceProvider().GetRequiredService<DataContext>();
        _ = sp.Init();
    }

    private void AddHanaCore(IServiceCollection services)
    {
        services
            // Configuration
            .Configure(PersistenceOptions);

        services
            // Serialization.
            .AddSingleton(IdentityGenerator)
            .AddSingleton<IApiSerializer, ApiSerializer>()
            .AddSingleton<ISafeSerializer, SafeSerializer>()
            .AddSingleton<IJsonSerializer, StandardJsonSerializer>()

            // Persistence
            .AddSingleton<DataContext>()

            // Logging
            .AddLogging();

    }
}