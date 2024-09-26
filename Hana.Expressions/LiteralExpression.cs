using Hana.Expressions.Contracts;
using Hana.Expressions.Helpers;
using Hana.Expressions.Models;
using JetBrains.Annotations;

namespace Hana.Expressions;


[UsedImplicitly]
public class LiteralExpressionHandler(IWellKnownTypeRegistry wellKnownTypeRegistry) : IExpressionHandler
{
    private readonly IWellKnownTypeRegistry _wellKnownTypeRegistry = wellKnownTypeRegistry;

    public ValueTask<object?> EvaluateAsync(Expression expression, Type returnType, ExpressionExecutionContext context, ExpressionEvaluatorOptions options)
    {
        var value = expression.Value.ConvertTo(returnType, new ObjectConverterOptions(WellKnownTypeRegistry: _wellKnownTypeRegistry));
        return ValueTask.FromResult(value);
    }
}