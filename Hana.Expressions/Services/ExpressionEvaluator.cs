using Hana.Expressions.Contracts;
using Hana.Expressions.Models;

namespace Hana.Expressions.Services;

public class ExpressionEvaluator(IExpressionDescriptorRegistry registry, IServiceProvider serviceProvider) : IExpressionEvaluator
{
    private readonly IExpressionDescriptorRegistry _registry = registry;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async ValueTask<T?> EvaluateAsync<T>(Expression expression, ExpressionExecutionContext context, ExpressionEvaluatorOptions? options = default)
    {
        return (T?)await EvaluateAsync(expression, typeof(T), context, options);
    }

    public async ValueTask<object?> EvaluateAsync(Expression expression, Type returnType, ExpressionExecutionContext context, ExpressionEvaluatorOptions? options = default)
    {
        var expressionType = expression.Type;
        var expressionDescriptor = _registry.Find(expressionType);

        if (expressionDescriptor == null)
            throw new Exception($"Could not find an descriptor for expression type \"{expressionType}\".");

        var handler = expressionDescriptor.HandlerFactory(_serviceProvider);
        options ??= new ExpressionEvaluatorOptions();
        return await handler.EvaluateAsync(expression, returnType, context, options);
    }
}