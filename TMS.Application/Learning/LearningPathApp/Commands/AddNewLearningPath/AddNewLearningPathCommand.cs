using MediatR;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.LearningPathApp.Commands.AddNewLearningPath;

public record AddNewLearningPathCommand(
    int PathId,
    int SellableItemId,
    string PathName,
    string Outcomes,
    string? Audience) : IRequest<ResultBase>;
