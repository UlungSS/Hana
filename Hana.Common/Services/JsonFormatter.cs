using System.Text.Json;
using System.Text.Json.Serialization;

using Hana.Common.Contracts;

namespace Hana.Common.Services;

public class JsonFormatter : IFormatter
{
    private readonly JsonSerializerOptions _options;
    
    public JsonFormatter()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new JsonStringEnumConverter());
    }
    
    public ValueTask<string> ToStringAsync(object value, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(value);
        return ValueTask.FromResult(json);
    }

    public ValueTask<object> FromStringAsync(string data, Type? returnType, CancellationToken cancellationToken = default)
    {
        var value = returnType != null ? JsonSerializer.Deserialize(data, returnType, _options)! : JsonSerializer.Deserialize<object>(data, _options)!;
        return ValueTask.FromResult(value);
    }
}