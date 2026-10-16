using MediatR;
using TMS.Application.Shared.Interfaces;
using TMS.Application.Shared.Models;
using TMS.Application.Shared.Models.DTOs;

namespace TMS.Application.Learning.CourseApp.Queries.GetAllCourses;

public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, PaginatedList<CourseDto>>
{
    public readonly ICourseRepository _courseRepository;

    public GetAllCoursesQueryHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    async Task<PaginatedList<CourseDto>> IRequestHandler<GetAllCoursesQuery, PaginatedList<CourseDto>>.Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        var courses = await _courseRepository.GetCoursesAsync(request.PageNumber, request.PageSize, request.SearchTerm);

        if (courses.TotalCount == 0)
            return new PaginatedList<CourseDto>([], 0, request.PageNumber, request.PageSize);

        return courses;
    }
}
