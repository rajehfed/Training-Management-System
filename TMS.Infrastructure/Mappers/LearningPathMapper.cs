using TMS.Application.Shared.Models.DTOs;
using TMS.Domain.LearningDomain.LearningPath;

namespace TMS.Infrastructure.Mappers;

public static class LearningPathMapper
{
    public static LearningPath ToDomain(this LearningPathDto dto)
    {
        return new LearningPath(
            dto.PathId,
            dto.SellableItemId,
            dto.PathName,
            new Outcomes(dto.Outcomes.Split(',')),
            dto.Audience,
            PathStatus.FromName(dto.Status),
            dto.CreatedAt,
            dto.UpdatedAt,
            [.. dto.Courses.Select(cip => new Domain.LearningDomain.CourseInPath.CourseInPath(cip.CourseId, cip.PathId, cip.OrderInPath))]
        );
    }
}
