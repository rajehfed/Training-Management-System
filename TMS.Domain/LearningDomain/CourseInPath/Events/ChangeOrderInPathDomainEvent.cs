using TMS.Domain.LearningDomain.CourseInPath;
using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.LearningDomain.CourseInPath.Events;

public record ChangeOrderInPathDomainEvent(CourseInPathId CourseInPathId) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
