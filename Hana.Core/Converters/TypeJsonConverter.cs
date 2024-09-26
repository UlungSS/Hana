using JetBrains.Annotations;
using System.Text.Json.Serialization;
using System.Text.Json;

using Hana.Expressions.Contracts;
using Hana.Extensions;


namespace Hana.Core.Converters;

[PublicAPI]
public class TypeJsonConverter(IWellKnownTypeRegistry wellKnownTypeRegistry) : JsonConverter<Type>
{
    private readonly IWellKnownTypeRegistry _wellKnownTypeRegistry = wellKnownTypeRegistry;

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(Type) || typeToConvert.FullName == "System.RuntimeType";
    }

    public override Type? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var typeAlias = reader.GetString()!;

        // Handle collection types.
        if (typeAlias.EndsWith("[]"))
        {
            var elementTypeAlias = typeAlias[..^"[]".Length];
            var elementType = _wellKnownTypeRegistry.TryGetType(elementTypeAlias, out var t) ? t : Type.GetType(elementTypeAlias)!;
            return typeof(List<>).MakeGenericType(elementType);
        }

        return _wellKnownTypeRegistry.TryGetType(typeAlias, out var type) ? type : Type.GetType(typeAlias);
    }

    public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
    {
        // Handle collection types.
        if (value is { IsGenericType: true, GenericTypeArguments.Length: 1 })
        {
            var elementType = value.GenericTypeArguments.First();
            var typedEnumerable = typeof(IEnumerable<>).MakeGenericType(elementType);

            if (typedEnumerable.IsAssignableFrom(value))
            {
                var elementTypeAlias = _wellKnownTypeRegistry.TryGetAlias(elementType, out var elementAlias) ? elementAlias : value.GetSimpleAssemblyQualifiedName();
                JsonSerializer.Serialize(writer, $"{elementTypeAlias}[]", options);
                return;
            }
        }

        var typeAlias = _wellKnownTypeRegistry.TryGetAlias(value, out var @alias) ? alias : value.GetSimpleAssemblyQualifiedName();
        JsonSerializer.Serialize(writer, typeAlias, options);
    }
}
