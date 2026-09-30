using MediatR;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.LearningPathApp.Commands.UpdateCourseOrderInPath;

public record UpdateCourseOrderInPathCommand(int PathId, int CourseId, int NewOrder) : IRequest<ResultBase>;
