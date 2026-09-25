using MediatR;
using TMS.Domain.LearningDomain.Course;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.CourseApp.Commands.CreateCourse;

public record CreateCourseCommand(
    int Id,
    int SellableItemId,
    string CourseName,
    string? Description,
    CourseDuration Duration) : IRequest<ResultBase>;
