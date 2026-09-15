using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.Learning.Course.Events;

public record CourseCreatedDomainEvent(int CourseId) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
