using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.Commercial.Offer.Events;

public record OfferInProgressDomainEvent(int OfferId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
