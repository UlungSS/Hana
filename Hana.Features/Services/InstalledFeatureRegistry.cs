using Hana.Features.Contracts;
using Hana.Features.Models;

namespace Hana.Features.Services;

public class InstalledFeatureRegistry : IInstalledFeatureRegistry
{
    private readonly Dictionary<string, FeatureDescriptor> _descriptors = [];

    public void Add(FeatureDescriptor descriptor) => _descriptors[descriptor.FullName] = descriptor;

    public IEnumerable<FeatureDescriptor> List() => _descriptors.Values;

    public FeatureDescriptor? Find(string fullName) => _descriptors.GetValueOrDefault(fullName);
}