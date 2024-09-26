using Hana.Expressions.Models;

namespace Hana.Expressions.Contracts;

public interface IExpressionHandler
{
    ValueTask<object?> EvaluateAsync(Expression expression, Type returnType, ExpressionExecutionContext context, ExpressionEvaluatorOptions options);
}