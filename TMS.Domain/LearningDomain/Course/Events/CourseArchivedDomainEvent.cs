using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.LearningDomain.Course.Events;

public record CourseArchivedDomainEvent(int CourseId) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
