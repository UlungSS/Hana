
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Hana.Core.Converters;

public class SafeValueConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => true;

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return new SafeValueConverter();
    }
}
