using TMS.Domain.Shared;

namespace TMS.Domain.Learning.CourseInPath;

public sealed class CourseInPathError
{
    public static readonly ErrorBase InvalidOrderInPath = ErrorBase.Validation(
        "InvalidOrderInPath",
        "Order in path must be a non-negative integer.");
}
