using MediatR;
using TMS.Application.Shared.Interfaces;
using TMS.Application.Shared.Models.DTOs;

namespace TMS.Application.Learning.CourseApp.Queries.GetCourseByName;

public class GetCourseByTitleQueryHandler : IRequestHandler<GetCourseByTitleQuery, CourseDto?>
{
    public readonly ICourseRepository _courseRepository;

    public GetCourseByTitleQueryHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<CourseDto?> Handle(GetCourseByTitleQuery request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByTitleAsync(request.CourseTitle, cancellationToken);
        if (course is null)
            return null;

        return new CourseDto(
            course.Id,
            course.SellableItemId,
            course.CourseName,
            course.Description,
            course.Duration.Minutes,
            course.Status.ToString(),
            course.CreatedAt,
            course.UpdatedAt);
    }
}
