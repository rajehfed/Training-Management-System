using TMS.Application.Shared.Models.DTOs;
using TMS.Domain.LearningDomain.Course;

namespace TMS.Infrastructure.Mappers;

public static class CourseMapper
{
    public static Course ToDomain(this CourseDto dto)
    {
        return new Course(
            dto.Id,
            dto.SellableItemId,
            dto.CourseName,
            dto.Description,
            new CourseDuration(dto.Duration.Value),
            CourseStatus.FromName(dto.Status),
            dto.CreatedAt,
            dto.UpdatedAt
        );
    }
}
