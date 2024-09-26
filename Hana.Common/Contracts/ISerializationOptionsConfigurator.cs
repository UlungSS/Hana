using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Hana.Common.Contracts;

public interface ISerializationOptionsConfigurator
{
    void Configure(JsonSerializerOptions options);
    
    IEnumerable<Action<JsonTypeInfo>> GetModifiers();
}