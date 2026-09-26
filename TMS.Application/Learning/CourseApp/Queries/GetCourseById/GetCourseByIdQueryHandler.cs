using MediatR;
using TMS.Application.Shared.Interfaces;
using TMS.Application.Shared.Models.DTOs;

namespace TMS.Application.Learning.CourseApp.Queries.GetCourseById;

public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, CourseDto?>
{
    public readonly ICourseRepository _courseRepository;

    public GetCourseByIdQueryHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<CourseDto?> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);

        if (course is null)
            return null;

        return new CourseDto(
            course.Id,
            course.SellableItemId,
            course.CourseName,
            course.Description,
            course.Status.ToString(),
            course.CreatedAt,
            course.UpdatedAt);
    }
}
