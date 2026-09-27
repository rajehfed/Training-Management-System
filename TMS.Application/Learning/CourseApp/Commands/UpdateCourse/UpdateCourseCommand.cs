using MediatR;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.CourseApp.Commands.UpdateCourse;

public record UpdateCourseCommand(
    int CourseId,
    string CourseTitle,
    string Description,
    int DurationInMinutes) : IRequest<ResultBase>;
