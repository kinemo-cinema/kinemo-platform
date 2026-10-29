namespace KinemoPlatform.API.Shared.Application.Patterns;

/// <summary>
///     Represents the outcome of an application-layer operation: either a <see cref="Success" />
///     carrying a value, or a <see cref="Failure" /> carrying an error.
/// </summary>
/// <remarks>
///     Application services return this type instead of throwing exceptions for expected failures
///     (not found, duplicates, ...). The REST layer maps each error to an HTTP response.
///     Unexpected failures (infrastructure errors) are still exceptions and are handled globally.
/// </remarks>
/// <typeparam name="TValue">The type of the value returned on success.</typeparam>
/// <typeparam name="TError">The type of the error returned on failure (usually an enum).</typeparam>
public abstract record Result<TValue, TError>
{
    /// <summary>
    ///     A successful result.
    /// </summary>
    /// <param name="Value">The value produced by the operation.</param>
    public sealed record Success(TValue Value) : Result<TValue, TError>;

    /// <summary>
    ///     A failed result.
    /// </summary>
    /// <param name="Error">The reason the operation failed.</param>
    public sealed record Failure(TError Error) : Result<TValue, TError>;

    /// <summary>
    ///     Whether this result is a <see cref="Success" />.
    /// </summary>
    public bool IsSuccess => this is Success;

    /// <summary>
    ///     Whether this result is a <see cref="Failure" />.
    /// </summary>
    public bool IsFailure => this is Failure;

    /// <summary>
    ///     Transforms the value of a successful result, leaving a failure untouched.
    /// </summary>
    public Result<TNext, TError> Map<TNext>(Func<TValue, TNext> onSuccess) => this switch
    {
        Success success => new Result<TNext, TError>.Success(onSuccess(success.Value)),
        Failure failure => new Result<TNext, TError>.Failure(failure.Error),
        _ => throw new InvalidOperationException("Invalid result state.")
    };

    /// <summary>
    ///     Collapses the result into a single value by handling both outcomes.
    /// </summary>
    public TResult Fold<TResult>(Func<TValue, TResult> onSuccess, Func<TError, TResult> onFailure) => this switch
    {
        Success s => onSuccess(s.Value),
        Failure f => onFailure(f.Error),
        _ => throw new InvalidOperationException("Invalid result state.")
    };

    /// <summary>
    ///     Runs the matching action for the outcome.
    /// </summary>
    public void Match(Action<TValue> onSuccess, Action<TError> onFailure)
    {
        switch (this)
        {
            case Success s:
                onSuccess(s.Value);
                break;
            case Failure f:
                onFailure(f.Error);
                break;
            default:
                throw new InvalidOperationException("Invalid result state.");
        }
    }
}