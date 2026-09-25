using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.LearningDomain.LearningPath.Events;

public record LearningPathUpdatedDomainEvent(int PathId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
