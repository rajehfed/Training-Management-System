using MediatR;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.LearningPathApp.Commands.UpdateLearningPathName;

public record UpdateLearningPathNameCommand(int PathId, string NewPathName) : IRequest<ResultBase>;
