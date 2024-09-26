namespace Hana.Expressions.Models;

public class ExpressionEvaluatorOptions
{
    public static readonly ExpressionEvaluatorOptions Empty = new();

    public IDictionary<string, object> Arguments { get; set; } = new Dictionary<string, object>();
}