using MediatR;
using TMS.Application.Shared.Models.DTOs;

namespace TMS.Application.Learning.CourseApp.EventHandlers.GetCourseById;

public record GetCourseByIdQuery(int CourseId) : IRequest<CourseDto?>;
