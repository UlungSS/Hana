namespace Hana.Core.Models;


public abstract class ArgumentDefinition
{
    public Type Type { get; set; } = typeof(object);

    public string Name { get; set; } = default!;

    public string DisplayName { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string Category { get; set; } = default!;
}