using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Hana.Common.Contracts;

public interface IJsonSerializer
{
    JsonSerializerOptions GetOptions();

    void ApplyOptions(JsonSerializerOptions options);

    [RequiresUnreferencedCode("The type is not known at compile time.")]
    string Serialize(object value);

    [RequiresUnreferencedCode("The type is not known at compile time.")]
    string Serialize(object value, Type type);

    string Serialize<T>(T value);

    [RequiresUnreferencedCode("The type is not known at compile time.")]
    object Deserialize(string json);

    [RequiresUnreferencedCode("The type is not known at compile time.")]
    object Deserialize(string json, Type type);

    T Deserialize<T>(string json);
}