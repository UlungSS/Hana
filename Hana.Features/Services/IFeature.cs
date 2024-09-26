using JetBrains.Annotations;

namespace Hana.Features.Services;


[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors | ImplicitUseTargetFlags.Members)]
public interface IFeature
{
    IModule Module { get; }

    void Configure();

    void ConfigureHostedServices();

    void Apply();

}