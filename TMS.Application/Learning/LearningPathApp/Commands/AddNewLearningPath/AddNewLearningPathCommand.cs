using MediatR;
using TMS.Domain.LearningDomain.LearningPath;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.LearningPathApp.Commands.AddNewLearningPath;

public record AddNewLearningPathCommand(
    int PathId,
    int SellableItemId,
    string PathName,
    string Outcomes,
    string? Audience,
    PathStatus Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt) : IRequest<ResultBase>;
