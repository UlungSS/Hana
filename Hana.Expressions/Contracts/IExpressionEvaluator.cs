using Hana.Expressions.Models;

namespace Hana.Expressions.Contracts;

public interface IExpressionEvaluator
{
    ValueTask<T?> EvaluateAsync<T>(Expression expression, ExpressionExecutionContext context, ExpressionEvaluatorOptions? options = default);

    ValueTask<object?> EvaluateAsync(Expression expression, Type returnType, ExpressionExecutionContext context, ExpressionEvaluatorOptions? options = default);
}