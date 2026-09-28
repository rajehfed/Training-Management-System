using MediatR;
using TMS.Application.Shared.Models.DTOs;

namespace TMS.Application.Learning.LearningPathApp.Queries.GetLearningPathByName;

public record GetLearningPathByNameQuery(string PathName) : IRequest<LearningPathDto?>;