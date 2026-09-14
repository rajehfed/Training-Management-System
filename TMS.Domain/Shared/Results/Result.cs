namespace TMS.Domain.Shared.Results;
public record Result<T> : ResultBase
{
    public T? Value { get; }
    private Result(T value) : base(true, null) => this.Value = value;
    private Result(string errorMessage) : base(false, errorMessage) { }
    public static implicit operator Result<T>(T value) => new(value);
    public static implicit operator Result<T>(string errorMessage) => new(errorMessage);
}
