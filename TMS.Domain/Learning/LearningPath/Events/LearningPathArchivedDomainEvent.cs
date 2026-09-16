using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.Learning.LearningPath.Events;

public record LearningPathArchivedDomainEvent(int PathId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
