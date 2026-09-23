using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.Commercial.Payment.Events;

public record PaymentSucceededDomainEvent(int Id, int OrderId, decimal Amount) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
