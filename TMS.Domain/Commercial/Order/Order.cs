using TMS.Domain.Commercial.Offer;
using TMS.Domain.Commercial.Order.Events;
using TMS.Domain.Shared;
using TMS.Domain.Shared.Results;

namespace TMS.Domain.Commercial.Order;

public class Order : AggregateRoot<int>
{
    public int StudentId { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.Draft;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public Order(int id) : base(id) { }

    public ResultBase ArchiveOrder()
    {
        if (this.Status.Equals(OrderStatus.Archived))
            return OrderError.AlreadyArchived;

        if (this.Status.Equals(OrderStatus.Draft))
            return OrderError.InvalidStatus;

        this.Status = OrderStatus.Archived;
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new ArchivingOrderDomainEvent(this.Id));
        return ResultBase.Success();
    }
    public ResultBase CompleteOrder()
    {
        if (this.Status.Equals(OrderStatus.Archived) || this.Status.Equals(OrderStatus.Cancelled))
            return OrderError.InvalidStatus;

        if (this.Status.Equals(OrderStatus.Completed))
            return OrderError.AlreadyComplleted;

        this.Status = OrderStatus.Completed;
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new OrderCompleteDomainEvent(this.Id));
        return ResultBase.Success();
    }
    public ResultBase CancelOrder()
    {
        if (this.Status.Equals(OrderStatus.Cancelled))
            return OrderError.AlreadyCancelled;

        if (this.Status.Equals(OrderStatus.Completed))
            return OrderError.InvalidStatus;

        this.Status = OrderStatus.Cancelled;
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new OrderCancelledDomainEvent(this.Id));
        return ResultBase.Success();
    }
}
