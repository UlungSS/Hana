namespace Hana.Common.Contracts;

public interface IFormatter
{
    ValueTask<string> ToStringAsync(object value, CancellationToken cancellationToken = default);
    
    ValueTask<object> FromStringAsync(string data, Type? returnType, CancellationToken cancellationToken = default);
}