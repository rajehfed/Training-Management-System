using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.Commercial.Offer.Events;

public record PublishingOfferDomainEvent(int OfferId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
