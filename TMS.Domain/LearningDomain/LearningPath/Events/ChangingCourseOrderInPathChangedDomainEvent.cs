using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.LearningDomain.LearningPath.Events;

public record ChangingCourseOrderInPathChangedDomainEvent(int PathId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
