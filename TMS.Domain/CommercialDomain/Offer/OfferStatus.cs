using TMS.Domain.Shared;

namespace TMS.Domain.Commercial.Offer;

public abstract class OfferStatus : Enumeration<OfferStatus>
{
    public static readonly OfferStatus Draft = new DraftStatus();
    public static readonly OfferStatus Published = new PublishedStatus();
    public static readonly OfferStatus Expired = new ExpiredStatus();
    public static readonly OfferStatus Archived = new ArchivedStatus();

    protected OfferStatus(int id, string name) : base(id, name) { }

    private sealed class DraftStatus() : OfferStatus(1, "Draft") { }
    private sealed class PublishedStatus() : OfferStatus(2, "Published") { }
    private sealed class ExpiredStatus() : OfferStatus(3, "Expired") { }
    private sealed class ArchivedStatus() : OfferStatus(4, "Archived") { }
}
