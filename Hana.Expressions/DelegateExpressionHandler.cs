using Hana.Expressions.Contracts;
using Hana.Expressions.Models;

namespace Hana.Expressions;


public class DelegateExpressionHandler : IExpressionHandler
{
    public async ValueTask<object?> EvaluateAsync(Expression expression, Type returnType, ExpressionExecutionContext context, ExpressionEvaluatorOptions options)
    {
        var value = expression.Value is Func<ExpressionExecutionContext, ValueTask<object?>> @delegate ? await @delegate(context) : default;
        return value;
    }
}