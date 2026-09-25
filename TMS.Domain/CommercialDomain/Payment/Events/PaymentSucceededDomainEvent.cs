using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.Commercial.Payment.Events;

public record PaymentSucceededDomainEvent(
    int PaymentId,
    int OrderId,
    decimal Amount) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
