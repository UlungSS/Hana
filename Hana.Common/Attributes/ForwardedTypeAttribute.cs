namespace Hana.Common;

public class ForwardedTypeAttribute(Type type) : Attribute
{
    public Type NewType { get; set; } = type;
}