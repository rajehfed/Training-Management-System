using MediatR;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.CourseApp.Commands.ArchiveCourse;

public record ArchiveCourseCommand(int CourseId) : IRequest<ResultBase>;
