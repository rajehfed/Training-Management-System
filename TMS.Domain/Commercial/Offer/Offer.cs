using TMS.Domain.Commercial.Offer.Events;
using TMS.Domain.Shared;
using TMS.Domain.Shared.Results;

namespace TMS.Domain.Commercial.Offer;

public class Offer : AggregateRoot<int>
{
    public int SellableItemId { get; private set; }
    public decimal ProductPrice { get; private set; }
    public OfferStatus Status { get; private set; } = OfferStatus.Draft;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public Offer() : base(default) { }

    public ResultBase ChangeSellableItem(int sellableItemId)
    {
        if (this.Status.Equals(OfferStatus.Archived) || this.Status.Equals(OfferStatus.Expired))
            return OfferError.InvalidStatus;

        if (sellableItemId < 0)
            return OfferError.InvalideSellableItem;

        this.SellableItemId = sellableItemId;
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new UpdatingOfferDomainEvent(this.Id));

        return ResultBase.Success();
    }
    public ResultBase ChangePrice(decimal newPrice)
    {
        if (Status.Equals(OfferStatus.Archived) || Status.Equals(OfferStatus.Expired))
            return OfferError.InvalidStatus;

        if (newPrice < 0)
            return OfferError.InvalidPrice;

        ProductPrice = newPrice;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new UpdatingOfferDomainEvent(Id));
        return ResultBase.Success();
    }
    public ResultBase ArchivingOffer()
    {
        if (this.Status.Equals(OfferStatus.Archived))
            return OfferError.AlreadyArchived;

        this.Status = OfferStatus.Archived;
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new ArchivingOfferDomainEvent(this.Id));
        return ResultBase.Success();
    }
    public ResultBase PublishOffer()
    {
        if (Status.Equals(OfferStatus.Published))
            return OfferError.AlreadyPublished;

        if (Status.Equals(OfferStatus.Archived) || Status.Equals(OfferStatus.Expired))
            return OfferError.InvalidStatus;

        Status = OfferStatus.Published;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new PublishingOfferDomainEvent(Id));
        return ResultBase.Success();
    }
    public ResultBase InDraftOffer()
    {
        if (this.Status.Equals(OfferStatus.Draft))
            return OfferError.AlreadyInDraft;

        if (this.Status.Equals(OfferStatus.Archived) || this.Status.Equals(OfferStatus.Expired))
            return OfferError.InvalidStatus;

        Status = OfferStatus.Draft;
        UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new InDraftOfferDomainEvent(this.Id));
        return ResultBase.Success();
    }
}
