namespace Knowledge.DDD.Demo.Kernel.Results;

public class Result
{
    protected Result(bool succeeded)
    {
        Succeeded = succeeded;
    }

    protected Result(bool succeeded, Exception exception) : this(succeeded)
    {
        Exception = exception;
    }

    public bool Succeeded { get; }

    public Exception? Exception { get; }

    public static Result Ok() => new(true);

    public static Result Fail<TException>(string exceptionMessage) where TException: Exception, new() => 
        new(false, (TException)Activator.CreateInstance(typeof(TException), exceptionMessage)!);

    public static Result Fail(Exception exception) => 
        new(false, exception);
}

public sealed class Result<T> : Result where T : notnull
{
    private Result(bool succeeded) : base(succeeded)
    {
    }

    private Result(bool succeeded, Exception exception) : base(succeeded, exception)
    {
    }

    private Result(bool succeeded, T result) : this(succeeded)
    {
        ResultValue = result;
    }

    public T? ResultValue { get; }

    public static Result<T> Ok(T result) => new(true, result);

    public new static Result<T> Fail<TException>(string exceptionMessage) where TException : Exception, new() => 
        new(false, (TException)Activator.CreateInstance(typeof(TException), exceptionMessage)!);

    public new static Result<T> Fail(Exception exception) => 
        new(false, exception);
}