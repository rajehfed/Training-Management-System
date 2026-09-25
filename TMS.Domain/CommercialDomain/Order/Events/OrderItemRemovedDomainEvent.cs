using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.Commercial.Order.Events;

public record OrderItemRemovedDomainEvent(int OrderId, int OfferId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
