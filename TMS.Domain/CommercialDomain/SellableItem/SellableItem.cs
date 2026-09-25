using TMS.Domain.Shared;

namespace TMS.Domain.Commercial.SellableItem;

public class SellableItem : AggregateRoot<int>
{
    public SellableItemType SellableItemType { get; private set; } = null!;
    public SellableItem() : base(default) { }

}
