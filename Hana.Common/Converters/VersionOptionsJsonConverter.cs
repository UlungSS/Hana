using System.Text.Json;
using System.Text.Json.Serialization;

using Hana.Common.Models;

namespace Hana.Common.Converters;

public class VersionOptionsJsonConverter : JsonConverter<VersionOptions>
{
    public override VersionOptions Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if(reader.TokenType == JsonTokenType.Number)
            return VersionOptions.SpecificVersion(reader.GetInt32());
        
        var textValue = reader.GetString();
        return string.IsNullOrWhiteSpace(textValue) ? VersionOptions.Published : VersionOptions.FromString(textValue);
    }

    public override void Write(Utf8JsonWriter writer, VersionOptions value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}