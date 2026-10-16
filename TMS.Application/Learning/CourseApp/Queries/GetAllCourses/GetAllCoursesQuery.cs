using MediatR;
using TMS.Application.Shared.Models;
using TMS.Application.Shared.Models.DTOs;

namespace TMS.Application.Learning.CourseApp.Queries.GetAllCourses;

public record GetAllCoursesQuery(
    int PageNumber = 0,
    int PageSize = 10,
    string? SearchTerm = null) : IRequest<PaginatedList<CourseDto>>;
