using MediatR;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.CourseApp.Commands.UpdateCourseTitle;

public record UpdateCourseTitleCommand(
    int CourseId,
    string CourseTitle) : IRequest<ResultBase>;