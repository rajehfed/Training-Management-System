namespace TMS.Domain.Shared.Results;

public record ResultBase
{
    public bool IsSuccess { get; }
    public ErrorBase? Error { get; }

    protected ResultBase(bool isSuccess, ErrorBase? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static ResultBase Success() => new(true, null);
    public static ResultBase Failure(ErrorBase error) => new(false, error);

    public static implicit operator ResultBase(ErrorBase error) => Failure(error);
}
