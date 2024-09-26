using System.Text.Json;

// ReSharper disable once CheckNamespace
namespace Hana.Extensions;

public static class JsonElementExtensions
{
    public static object? GetValue(this JsonElement jsonElement)
    {
        return jsonElement.ValueKind switch
        {
            JsonValueKind.String => jsonElement.GetString(),
            JsonValueKind.Number => jsonElement.GetDecimal(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Undefined => null,
            JsonValueKind.Null => null,
            JsonValueKind.Object => jsonElement.GetRawText(),
            JsonValueKind.Array => jsonElement.GetRawText(),
            _ => jsonElement.GetRawText()
        };
    }
}