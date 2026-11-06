using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.LearningDomain.LearningPath.Events;

public record RemovingCourseFromPathDomainEvent(int LearningPathId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}