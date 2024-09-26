using JetBrains.Annotations;

namespace Hana.Features.Models;


[PublicAPI]
public class FeatureDescriptor
{

    public FeatureDescriptor()
    {
    }

    public FeatureDescriptor(string name, string ns, string displayName, string? description = default)
    {
        Name = name;
        Namespace = ns;
        DisplayName = displayName;
        Description = description ?? "";
    }

    public string Name { get; set; } = default!;
    public string Namespace { get; set; } = default!;
    public string FullName => $"{Namespace}.{Name}";
    public string DisplayName { get; set; } = default!;
    public string Description { get; set; } = "";
}