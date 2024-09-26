using System.Text.Json.Serialization.Metadata;
using System.Text.Json;

using Hana.Common.Contracts;

namespace Hana.Core.Api.Abstractions;

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
