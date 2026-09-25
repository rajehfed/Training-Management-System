using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.Commercial.Order.Events;

public record ArchivingOrderDomainEvent(int OrderId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
