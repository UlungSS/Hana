using System.Text.Json.Serialization;
using System.Text.Json;

using Hana.Expressions.Contracts;
using Hana.Expressions.Models;

namespace Hana.Core.Converters;


public class ExpressionJsonConverterFactory(IExpressionDescriptorRegistry expressionDescriptorRegistry) : JsonConverterFactory
{
    private readonly IExpressionDescriptorRegistry _expressionDescriptorRegistry = expressionDescriptorRegistry;

    public override bool CanConvert(Type typeToConvert) => typeof(Expression).IsAssignableFrom(typeToConvert);


    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return new ExpressionJsonConverter(_expressionDescriptorRegistry);
    }
}
