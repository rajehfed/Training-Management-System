using MediatR;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.LearningPathApp.Commands.ArchiveLearningPath;

public record ArchiveLearningPathCommand(int PathId) : IRequest<ResultBase>;