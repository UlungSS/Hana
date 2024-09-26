namespace Hana.Expressions.Models;


public class Result
{
    internal Result(bool success, object? value, Exception? exception)
    {
        Success = success;
        Value = value;
        Exception = exception;
    }

    public bool Success { get; }

    public object? Value { get; }

    public Exception? Exception { get; }

    public Result OnSuccess(Action<object?> successHandler)
    {
        if (Success)
            successHandler(Value);

        return this;
    }

    public Result OnFailure(Action<Exception> failureHandler)
    {
        if (Exception != null)
            failureHandler(Exception);

        return this;
    }
}