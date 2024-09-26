using System.Linq.Expressions;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Hana.Extensions;


public static class PropertyAccessorExtensions
{
    public static void SetPropertyValue(this object target, string propertyName, object? value)
    {
        var property = target.GetType().GetProperty(propertyName)!;
        property.SetValue(target, value);
    }

    public static void SetPropertyValue<T, TProperty>(this T target, Expression<Func<T, TProperty>> expression, TProperty value)
    {
        var property = expression.GetProperty();

        property?.SetValue(target, value, null);
    }

    public static TProperty? GetPropertyValue<T, TProperty>(this T target, Expression<Func<T, TProperty>> expression)
    {
        var property = expression.GetProperty();
        return (TProperty?)property?.GetValue(target);
    }

    public static string GetPropertyName<T, TProperty>(this Expression<Func<T, TProperty>> expression) => expression.GetProperty()!.Name;

    public static PropertyInfo? GetProperty<T, TProperty>(this Expression<Func<T, TProperty>> expression) =>
    expression.Body is MemberExpression memberExpression
        ? memberExpression.Member as PropertyInfo
        : expression.Body is UnaryExpression unaryExpression
            ? unaryExpression.Operand is MemberExpression unaryMemberExpression
                ? unaryMemberExpression.Member as PropertyInfo
                : default
            : default;
}