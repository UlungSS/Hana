using Hana.Core.Api.Features;

// ReSharper disable once CheckNamespace
namespace Hana.Extensions;


public static class CoreApiFeatureExtensions
{
    public static CoreApiFeature AddFastEndpointsAssembly<TMarker>(this CoreApiFeature feature)
    {
        feature.Module.AddFastEndpointsAssembly<TMarker>();
        return feature;
    }
}