using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hana.Expressions.Models;


public class ObjectLiteral : MemoryBlockReference
{

    [JsonConstructor]
    public ObjectLiteral()
    {
    }

    public ObjectLiteral(string? value)
    {
        Value = value;
    }


    public string? Value { get; }

    public static ObjectLiteral From<T>(T value) => new ObjectLiteral<T>(value);
}


public class ObjectLiteral<T> : ObjectLiteral
{
    public ObjectLiteral()
    {
    }

    public ObjectLiteral(T value) : base(JsonSerializer.Serialize(value!))
    {
    }
}