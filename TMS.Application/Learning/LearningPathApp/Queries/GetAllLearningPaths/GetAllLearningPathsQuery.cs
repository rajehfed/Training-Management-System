using MediatR;
using TMS.Application.Shared.Models.DTOs;

namespace TMS.Application.Learning.LearningPathApp.Queries.GetAllLearningPaths;

public record GetAllLearningPathsQuery : IRequest<List<LearningPathDto>?>;
