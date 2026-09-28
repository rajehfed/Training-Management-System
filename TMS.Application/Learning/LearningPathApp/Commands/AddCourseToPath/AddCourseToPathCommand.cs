using MediatR;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.LearningPathApp.Commands.AddCourseToPath;

public record AddCourseToPathCommand(
    int LearningPathId,
    int CourseId,
    int OrderInPath) : IRequest<ResultBase>;