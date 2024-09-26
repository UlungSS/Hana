using System.Text.Json;

namespace Hana.Core.Contracts;

public interface IApiSerializer
{
    string Serialize(object model);

    object Deserialize(string serializedModel);

    T Deserialize<T>(string serializedModel);

    JsonSerializerOptions GetOptions();

    JsonSerializerOptions ApplyOptions(JsonSerializerOptions options);
}