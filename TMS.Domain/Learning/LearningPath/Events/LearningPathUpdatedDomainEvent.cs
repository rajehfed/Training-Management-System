using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.Learning.LearningPath.Events;

public record LearningPathUpdatedDomainEvent(int PathId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
