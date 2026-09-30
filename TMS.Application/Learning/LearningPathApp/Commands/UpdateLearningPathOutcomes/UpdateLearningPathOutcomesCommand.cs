using MediatR;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.LearningPathApp.Commands.UpdateLearningPathOutcomes;

public record UpdateLearningPathOutcomesCommand(
    int PathId,
    IEnumerable<string> Outcomes) : IRequest<ResultBase>;