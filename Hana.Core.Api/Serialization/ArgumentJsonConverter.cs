using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

using Hana.Expressions.Contracts;
using Hana.Expressions.Extensions;
using Hana.Extensions;

using Hana.Core.Models;

namespace Hana.Core.Api.Serialization;

public class ArgumentJsonConverter(IWellKnownTypeRegistry wellKnownTypeRegistry) : JsonConverter<ArgumentDefinition>
{
    private readonly IWellKnownTypeRegistry _wellKnownTypeRegistry = wellKnownTypeRegistry;

    public override void Write(Utf8JsonWriter writer, ArgumentDefinition value, JsonSerializerOptions options)
    {
        var newOptions = new JsonSerializerOptions(options);
        newOptions.Converters.RemoveWhere(x => x is ArgumentJsonConverterFactory);

        var jsonObject = (JsonObject)JsonSerializer.SerializeToNode(value, value.GetType(), newOptions)!;
        var isArray = value.Type.IsCollectionType();
        jsonObject["isArray"] = isArray;
        jsonObject["type"] = _wellKnownTypeRegistry.GetAliasOrDefault(isArray ? value.Type.GetCollectionElementType() : value.Type);

        JsonSerializer.Serialize(writer, jsonObject, newOptions);
    }

    public override ArgumentDefinition Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonObject = (JsonObject)JsonNode.Parse(ref reader)!;
        var isArray = jsonObject["isArray"]?.GetValue<bool>() ?? false;
        var typeName = jsonObject["type"]!.GetValue<string>();
        var type = _wellKnownTypeRegistry.GetTypeOrDefault(typeName);

        if (isArray)
            type = type.MakeCollectionType();

#pragma warning disable CA1869 // Cache and reuse 'JsonSerializerOptions' instances
        var newOptions = new JsonSerializerOptions(options);
#pragma warning restore CA1869 // Cache and reuse 'JsonSerializerOptions' instances
        newOptions.Converters.RemoveWhere(x => x is ArgumentJsonConverterFactory);
        var inputDefinition = (ArgumentDefinition)jsonObject.Deserialize(typeToConvert, newOptions)!;
        inputDefinition.Type = type;

        return inputDefinition;
    }
}