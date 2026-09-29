using MediatR;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.LearningPathApp.Commands.UpdateLearningPath;

public record UpdateLearningPathCommand(
    int PathId,
    int SellableItemId,
    string PathName,
    string Outcomes,
    string? Audience) : IRequest<ResultBase>;