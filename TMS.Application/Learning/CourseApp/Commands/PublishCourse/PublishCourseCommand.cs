using MediatR;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.CourseApp.Commands.PublishCourse;

public record PublishCourseCommand(int CourseId) : IRequest<ResultBase>;
