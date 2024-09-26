using System.Linq.Expressions;

namespace Hana.Common.Entities;

public class OrderDefinition<T, TProp>
{
    public OrderDefinition()
    {
    }
    public OrderDefinition(Expression<Func<T, TProp>> keySelector, OrderDirection direction)
    {
        KeySelector = keySelector;
        Direction = direction;
    }
    
    public OrderDirection Direction { get; set; }
    
    public Expression<Func<T, TProp>> KeySelector { get; set; } = default!;
}