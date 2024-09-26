
using Hana.Common.Serialization;
using Hana.Core.Contracts;
using Hana.Core.Converters;
using Hana.Expressions.Contracts;
using Hana.Expressions.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Hana.Core.Serializer;

public class SafeSerializer(IServiceProvider serviceProvider) : ConfigurableSerializer(serviceProvider), ISafeSerializer
{

    [RequiresUnreferencedCode("The type T may be trimmed.")]
    public ValueTask<string> SerializeAsync(object? value, CancellationToken cancellationToken = default)
    {
        var options = GetOptions();
        return ValueTask.FromResult(JsonSerializer.Serialize(value, options));
    }

    [RequiresUnreferencedCode("The type T may be trimmed.")]
    public ValueTask<JsonElement> SerializeToElementAsync(object? value, CancellationToken cancellationToken = default)
    {
        var options = GetOptions();
        return new(JsonSerializer.SerializeToElement(value, options));
    }

    [RequiresUnreferencedCode("The type T may be trimmed.")]
    public ValueTask<T> DeserializeAsync<T>(string json, CancellationToken cancellationToken = default)
    {
        var options = GetOptions();
        return new(JsonSerializer.Deserialize<T>(json, options)!);
    }

    [RequiresUnreferencedCode("The type T may be trimmed.")]
    public ValueTask<T> DeserializeAsync<T>(JsonElement element, CancellationToken cancellationToken = default)
    {
        var options = GetOptions();
        return new(element.Deserialize<T>(options)!);
    }

    protected override void AddConverters(JsonSerializerOptions options)
    {
        var expressionDescriptorRegistry = ServiceProvider.GetRequiredService<IExpressionDescriptorRegistry>();

        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        options.Converters.Add(new TypeJsonConverter(WellKnownTypeRegistry.CreateDefault()));
        options.Converters.Add(new SafeValueConverterFactory());
        options.Converters.Add(new ExpressionJsonConverterFactory(expressionDescriptorRegistry));
    }
}
