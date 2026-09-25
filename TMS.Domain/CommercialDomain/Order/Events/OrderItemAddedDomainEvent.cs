using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.Commercial.Order.Events;

public record OrderItemAddedDomainEvent(int OrderId, int OfferID) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}