using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.LearningDomain.LearningPath.Events;

public record LearningPathPublishedDomainEvent(int PathId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
