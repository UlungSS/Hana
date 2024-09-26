using Hana.Expressions.Helpers;

namespace Hana.Expressions.Models;

public class MemoryBlockReference
{
    public MemoryBlockReference()
    {
    }

    public MemoryBlockReference(string id) => Id = id;

    public string Id { get; set; } = default!;

    public virtual MemoryBlock Declare() => new();

    public bool IsDefined(MemoryRegister register) => register.HasBlock(Id);

    public object? Get(MemoryRegister memoryRegister) => GetBlock(memoryRegister).Value;

    public T? Get<T>(MemoryRegister memoryRegister) => Get(memoryRegister)!.ConvertTo<T>();

    public object? Get(ExpressionExecutionContext context) => context.Get(this);

    public T? Get<T>(ExpressionExecutionContext context) => Get(context)!.ConvertTo<T>();

    public bool TryGet(ExpressionExecutionContext context, out object? value) => context.TryGet(this, out value);

    public void Set(MemoryRegister memoryRegister, object? value, Action<MemoryBlock>? configure = default)
    {
        var block = GetBlock(memoryRegister);
        block.Value = value;
        configure?.Invoke(block);
    }

    public void Set(ExpressionExecutionContext context, object? value, Action<MemoryBlock>? configure = default) => context.Set(this, value, configure);

    public MemoryBlock GetBlock(MemoryRegister memoryRegister) => memoryRegister.TryGetBlock(Id, out var location) ? location : memoryRegister.Declare(this);
}

public class MemoryBlockReference<T> : MemoryBlockReference
{
}