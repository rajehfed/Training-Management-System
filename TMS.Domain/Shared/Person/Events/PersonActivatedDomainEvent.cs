using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.Shared.Person.Events;

public record PersonActivatedDomainEvent(int PersonId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
