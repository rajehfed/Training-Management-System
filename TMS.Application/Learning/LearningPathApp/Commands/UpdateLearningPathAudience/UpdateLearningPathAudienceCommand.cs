using MediatR;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.LearningPathApp.Commands.UpdateLearningPathAudience;

public record UpdateLearningPathAudienceCommand(int PathId, string NewAudience) : IRequest<ResultBase>;
