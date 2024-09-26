namespace Hana.Expressions.Models;


public class MemoryBlock
{
    public MemoryBlock()
    {
    }

    public MemoryBlock(object? value, object? metadata = default)
    {
        Value = value;
        Metadata = metadata;
    }


    public object? Value { get; set; }

    public object? Metadata { get; set; }
}