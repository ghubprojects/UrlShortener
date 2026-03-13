namespace UrlShortener.Shared.Results;

/// <summary>
/// Represents a result of an operation with a value
/// </summary>
public class Result<T> : Result
{
    private readonly T? _value;

    public T Value
    {
        get
        {
            if (IsFailure)
                throw new InvalidOperationException("Cannot access Value when result is failure.");

            return _value!;
        }
    }

    protected Result(bool isSuccess, T? value, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    public static Result<T> Success(T value)
        => new(true, value, Error.None);

    public new static Result<T> Failure(Error error)
        => new(false, default, error);

    public static implicit operator Result<T>(T value)
        => Success(value);
}