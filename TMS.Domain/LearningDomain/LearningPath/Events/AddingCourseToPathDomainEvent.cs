using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.LearningDomain.LearningPath.Events;

public record AddingCourseToPathDomainEvent(int PathId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
