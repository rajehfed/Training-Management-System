using MediatR;
using TMS.Domain.LearningDomain.LearningPath;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.LearningPathApp.Commands.AddNewLearningPath;

public record AddNewLearningPathCommand(
    string PathName,
    string Outcomes,
    string? Audience) : IRequest<ResultBase>;
