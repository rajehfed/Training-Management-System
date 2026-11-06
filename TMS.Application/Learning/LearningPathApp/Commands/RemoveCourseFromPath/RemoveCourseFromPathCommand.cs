using MediatR;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.LearningPathApp.Commands.RemoveCourseFromPath;

public record RemoveCourseFromPathCommand(int PathId, int CourseId) : IRequest<ResultBase>;