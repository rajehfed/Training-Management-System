using MediatR;
using TMS.Application.Shared.Models.DTOs;

namespace TMS.Application.Learning.LearningPathApp.Queries.GetLearningPathById;

public record GetLearningPathByIdQuery(int PathId) : IRequest<LearningPathDto?>;
