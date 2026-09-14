namespace TMS.Domain.Shared.Results;

public record ResultBase
{
    public bool IsSuccess { get; }
    public Error? ErrorMessage { get; }

    protected ResultBase(bool isSuccess, string? errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static ResultBase Success() => new(true, null);
    public static ResultBase Failure(string errorMessage) => new(false, errorMessage);

    public static implicit operator ResultBase(string errorMessage) => Failure(errorMessage);
}
