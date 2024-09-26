using System.Text.Json;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Hana.Extensions;

public static class JsonSerializerOptionsExtensions
{
    public static JsonSerializerOptions WithConverters(this JsonSerializerOptions options, params JsonConverter[] converters)
    {
        foreach (var converter in converters)
            options.Converters.Add(converter);

        return options;
    }

    public static JsonSerializerOptions Clone(this JsonSerializerOptions options)
    {
        return new JsonSerializerOptions(options);
    }
}