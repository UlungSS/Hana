using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hana.Common.Converters;

public class DecimalJsonConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
            return reader.GetDecimal();

        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString()!;
            return decimal.Parse(value, CultureInfo.InvariantCulture);
        }

        throw new JsonException("Expected number or string.");
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        // Write the decimal as a JSON number
        writer.WriteNumberValue(value);
    }
}