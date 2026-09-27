using MediatR;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.CourseApp.Commands.UpdateCourseDescription;

public record UpdateCourseDescriptionCommand(int CourseId, string Description) : IRequest<ResultBase>;
