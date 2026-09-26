using MediatR;
using TMS.Application.Shared.Models.DTOs;

namespace TMS.Application.Learning.CourseApp.Queries.GetCourseById;

public record GetCourseByIdQuery(int CourseId) : IRequest<CourseDto?>;
