using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.Learning.CourseInPath.Events;

public record ChangeOrderInPathDomainEvent(CourseInPathId CourseInPathId) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
