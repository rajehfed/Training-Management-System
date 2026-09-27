using MediatR;
using TMS.Application.Shared.Models.DTOs;

namespace TMS.Application.Learning.CourseApp.Queries.GetCourseByName;

public record GetCourseByTitleQuery(string CourseTitle) : IRequest<CourseDto?>;
