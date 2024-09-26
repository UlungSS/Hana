namespace Hana.Features.Attributes;


[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DependencyOfAttribute(Type type) : Attribute
{
    public Type Type { get; set; } = type;
}