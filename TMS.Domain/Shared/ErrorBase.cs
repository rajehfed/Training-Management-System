namespace TMS.Domain.Shared;

public enum ErrorType
{
    Failure,
    ValidationError,
    NotFoundError,
    UnauthorizedError,
    ForbiddenError,
    ConflictError
}
public record ErrorBase(string ErrorCode, string Description, ErrorType Type)
{
    public static readonly ErrorBase None = new(String.Empty, String.Empty, ErrorType.Failure);

    public static ErrorBase Validation(string code, string description) =>
        new(code, description, ErrorType.ValidationError);
    public static ErrorBase NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFoundError);
    public static ErrorBase Unauthorized(string code, string description) =>
        new(code, description, ErrorType.UnauthorizedError);
    public static ErrorBase Forbidden(string code, string description) =>
        new(code, description, ErrorType.ForbiddenError);
    public static ErrorBase Conflict(string code, string description) =>
        new(code, description, ErrorType.ConflictError);
}
