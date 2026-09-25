using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.Commercial.Order.Events;

public record OrderPaidDomainEvent(int OrderId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
