using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.Commercial.Payment.Events;

public record PaymentFailedDomainEvent(int Id, int OrderId, string FailureReason) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
