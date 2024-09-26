using System.Text.Json.Serialization;

namespace Hana.Expressions.Models;

public partial class Expression
{
    [JsonConstructor]
    public Expression()
    {
    }

    public Expression(string type, object? value)
    {
        Type = type;
        Value = value;
    }

    public string Type { get; set; } = default!;

    public object? Value { get; set; }

    public static Expression LiteralExpression(object? value) => new("Literal", value);

    public static Expression DelegateExpression(Func<ExpressionExecutionContext, ValueTask<object?>> value) => new()
    {
        Type = "Delegate",
        Value = value
    };

    public static Expression DelegateExpression<T>(Func<ExpressionExecutionContext, ValueTask<T>> value) => DelegateExpression(async context => (object?)await value(context));

#pragma warning disable CA2012 // Use ValueTasks correctly
    public static Expression DelegateExpression<T>(Func<ValueTask<T>> value) => DelegateExpression(_ => ValueTask.FromResult<object?>(value()));
#pragma warning restore CA2012 // Use ValueTasks correctly

    public static Expression DelegateExpression<T>(Func<ExpressionExecutionContext, T> value) => DelegateExpression(context => ValueTask.FromResult<object?>(value(context)));

    public static Expression DelegateExpression<T>(Func<T> value) => DelegateExpression(_ => ValueTask.FromResult<object?>(value()));
}