using MediatR;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.LearningPathApp.Commands.PublishLearningPath;

public record PublishLearningPathCommand(int PathId) : IRequest<ResultBase>;