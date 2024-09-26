using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

using Hana.Common.Contracts;

namespace Hana.Common.Serialization;

public class StandardJsonSerializer(IServiceProvider serviceProvider) : ConfigurableSerializer(serviceProvider), IJsonSerializer
{

    [RequiresUnreferencedCode("The type is not known at compile time.")]
    public string Serialize(object value)
    {
        var options = GetOptions();
        return JsonSerializer.Serialize(value, options);
    }

    [RequiresUnreferencedCode("The type is not known at compile time.")]
    public string Serialize(object? value, Type type)
    {
        var options = GetOptions();
        return JsonSerializer.Serialize(value, type, options);
    }

    public string Serialize<T>(T value)
    {
        return Serialize(value, typeof(T));
    }

    [RequiresUnreferencedCode("The type is not known at compile time.")]
    public object Deserialize(string json)
    {
        var options = GetOptions();
        return JsonSerializer.Deserialize<object>(json, options)!;
    }

    [RequiresUnreferencedCode("The type is not known at compile time.")]
    public object Deserialize(string json, Type type)
    {
        var options = GetOptions();
        return JsonSerializer.Deserialize(json, type, options)!;
    }

    public T Deserialize<T>(string json)
    {
        return (T)Deserialize(json, typeof(T));
    }
}