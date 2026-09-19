using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.Shared.Person.Events;

public record PersonArchivedDomainEvent(int PersonId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
