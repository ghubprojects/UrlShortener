namespace UrlShortener.Shared.Results;

/// <summary>
/// Represents a result of an operation with success/failure indication
/// </summary>
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success()
        => new(true, Error.None);
    public static Result<T> Success<T>(T value)
        => Result<T>.Success(value);

    public static Result Failure(Error error)
        => new(false, error);
    public static Result<T> Failure<T>(Error error)
        => Result<T>.Failure(error);

    public Result<TOut> ToFailure<TOut>()
        => Failure<TOut>(Error);
}