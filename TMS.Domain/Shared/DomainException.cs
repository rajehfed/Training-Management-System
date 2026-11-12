namespace TMS.Domain.Shared;

public class DomainException : Exception
{
    public ErrorBase Error { get; }
    public DomainException(ErrorBase error) : base(error.Description) => Error = error;
}
