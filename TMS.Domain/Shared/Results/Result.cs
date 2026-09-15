namespace TMS.Domain.Shared.Results;
public record Result<T> : ResultBase
{
    public T? Value { get; }
    private Result(T value) : base(true, ErrorBase.None) => this.Value = value;
    private Result(ErrorBase error) : base(false, error) { }
    public static implicit operator Result<T>(T value) => new(value);
    public static implicit operator Result<T>(ErrorBase error) => new(error);
}
