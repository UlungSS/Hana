using System.Text.Json.Serialization.Metadata;

using Hana.Extensions;

namespace Hana.Common.Serialization;

public class ModifiableJsonTypeInfoResolver : DefaultJsonTypeInfoResolver
{
    public ModifiableJsonTypeInfoResolver(IEnumerable<Action<JsonTypeInfo>> modifiers)
    {
        Modifiers.AddRange(modifiers);
    }
}