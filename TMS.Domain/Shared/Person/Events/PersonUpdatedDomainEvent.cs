using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.Shared.Person.Events;

public record PersonUpdatedDomainEvent(int PersonId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
