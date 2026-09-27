using MediatR;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.CourseApp.Commands.UpdateCourseDuration;

public record UpdateCourseDurationCommand(int CourseId, int DurationInMinutes) : IRequest<ResultBase>;