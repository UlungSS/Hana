using Hana.Expressions.Models;

namespace Hana.Expressions.Contracts;

public interface IExpressionDescriptorProvider
{
    IEnumerable<ExpressionDescriptor> GetDescriptors();
}