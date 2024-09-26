using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

using Hana.Common.Contracts;

namespace Hana.Core;

public abstract class SerializationOptionsConfiguratorBase : ISerializationOptionsConfigurator
{
    public virtual void Configure(JsonSerializerOptions options)
    {
    }

    public virtual IEnumerable<Action<JsonTypeInfo>> GetModifiers()
    {
        yield break;
    }
}