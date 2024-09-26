namespace Hana.Expressions.Models;


public class MemoryRegister(IDictionary<string, MemoryBlock>? blocks = default)
{

    public IDictionary<string, MemoryBlock> Blocks { get; } = blocks ?? new Dictionary<string, MemoryBlock>();

    public bool IsDeclared(MemoryBlockReference reference) => HasBlock(reference.Id);

    public bool HasBlock(string id) => Blocks.ContainsKey(id);

    public bool TryGetBlock(string id, out MemoryBlock block)
    {
        return Blocks.TryGetValue(id, out block!);
    }

    public void Declare(IEnumerable<MemoryBlockReference> references)
    {
        foreach (var reference in references)
            Declare(reference);
    }

    public MemoryBlock Declare(MemoryBlockReference blockReference)
    {
        if (Blocks.TryGetValue(blockReference.Id, out var block))
            return block;

        block = blockReference.Declare();
        Blocks[blockReference.Id] = block;
        return block;
    }
}