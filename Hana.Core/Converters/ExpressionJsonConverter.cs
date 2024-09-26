using System.Text.Json.Serialization;
using System.Text.Json;

using Hana.Expressions.Contracts;
using Hana.Expressions.Models;

namespace Hana.Core.Converters;

public class ExpressionJsonConverter(IExpressionDescriptorRegistry expressionDescriptorRegistry) : JsonConverter<Expression>
{
    private readonly IExpressionDescriptorRegistry _expressionDescriptorRegistry = expressionDescriptorRegistry;

    //public override bool CanConvert(Type typeToConvert) => typeof(Input).IsAssignableFrom(typeToConvert);

    public override Expression Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!JsonDocument.TryParseValue(ref reader, out var doc))
            return default!;

        var expressionElement = doc.RootElement;
        var expressionTypeNameElement = expressionElement.TryGetProperty("type", out var expressionTypeNameElementValue) ? expressionTypeNameElementValue : default;
        var expressionTypeName = expressionTypeNameElement.ValueKind != JsonValueKind.Undefined ? expressionTypeNameElement.GetString() ?? "Literal" : default;
        var expressionDescriptor = expressionTypeName != null ? _expressionDescriptorRegistry.Find(expressionTypeName) : default;
        var memoryBlockReference = expressionDescriptor?.MemoryBlockReferenceFactory?.Invoke();

        if (memoryBlockReference == null)
            return default!;

        var memoryBlockType = memoryBlockReference.GetType();
        var context = new ExpressionSerializationContext(expressionTypeName!, expressionElement, options, memoryBlockType);
        var expression = expressionDescriptor!.Deserialize(context);

        return expression;
    }

    public override void Write(Utf8JsonWriter writer, Expression value, JsonSerializerOptions options)
    {
        var expression = value;

        if (expression == null)
        {
            writer.WriteNullValue();
            return;
        }

        var expressionType = expression.Type;
        var expressionDescriptor = (expressionType != null ? _expressionDescriptorRegistry.Find(expressionType) : null) ?? throw new JsonException($"Could not find an expression descriptor for expression type '{expressionType}'.");
        var expressionValue = expressionDescriptor.IsSerializable ? expression.Value : null;

        var model = new
        {
            Type = expressionType,
            Value = expressionValue
        };

        JsonSerializer.Serialize(writer, model, options);
    }
}
