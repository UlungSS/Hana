using System.Text.Json;

using Hana.Expressions.Contracts;
using Hana.Extensions;

namespace Hana.Expressions.Models;

public class ExpressionDescriptor
{
    public ExpressionDescriptor()
    {
        // Default deserialization function.
        Deserialize = context =>
        {
            var expression = new Expression(context.ExpressionType, null);

            if (context.JsonElement.ValueKind == JsonValueKind.Object)
                if (context.JsonElement.TryGetProperty("value", out var expressionValueElement))
                    expression.Value = expressionValueElement.GetValue();

            return expression;
        };
    }

    public string Type { get; init; } = default!;

    public string DisplayName { get; set; } = default!;

    public bool IsSerializable { get; set; } = true;

    public bool IsBrowsable { get; set; } = true;

    public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

    public Func<IServiceProvider, IExpressionHandler> HandlerFactory { get; set; } = default!;

    public Func<MemoryBlockReference> MemoryBlockReferenceFactory { get; set; } = () => new MemoryBlockReference();

    public Func<ExpressionSerializationContext, Expression> Deserialize { get; set; } = default!;
}