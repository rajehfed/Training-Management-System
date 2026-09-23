using TMS.Domain.Shared;

namespace TMS.Domain.Commercial;

public class OrderItem : Entity<int>
{
    public int OrderId { get; set; }
    public int OfferId { get; set; }
    public decimal PurchasedPrice { get; private set; }
    public OrderItem(int id) : base(id) { }

    internal OrderItem(
        int id,
        int orderId,
        int offerId,
        decimal purchasedPrice) : base(id)
    {
        if (offerId <= 0)
            throw new ArgumentOutOfRangeException(nameof(offerId), "OfferId must be greater than zero.");

        if (purchasedPrice < 0)
            throw new ArgumentOutOfRangeException(nameof(purchasedPrice), "PurchasedPrice cannot be negative.");

        OrderId = orderId;
        OfferId = offerId;
        PurchasedPrice = purchasedPrice;
    }
}
