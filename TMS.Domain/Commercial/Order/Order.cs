using TMS.Domain.Commercial.Offer;
using TMS.Domain.Commercial.Order.Events;
using TMS.Domain.Commercial.Payment;
using TMS.Domain.Shared;
using TMS.Domain.Shared.Results;

namespace TMS.Domain.Commercial.Order;

public class Order : AggregateRoot<int>
{
    public int StudentId { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.Draft;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private List<OrderItem> _orderItems = [];
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    protected Order() : base(default) { }
    public Order(int id, int studentId) : base(id)
    {
        if (studentId <= 0)
            throw new ArgumentOutOfRangeException(nameof(studentId), "StudentId must be greater than zero.");

        StudentId = studentId;
        Status = OrderStatus.Draft;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

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

    public ResultBase AddItem(int offerId, decimal price)
    {
        if (Status.Equals(OrderStatus.Archived) || Status.Equals(OrderStatus.Completed) || Status.Equals(OrderStatus.Cancelled))
            return OrderError.InvalidStatus;

        if (offerId <= 0)
            return OrderError.InvalidOfferId;

        if (price < 0)
            return OrderError.InvalidPrice;

        if (_orderItems.Any(item => item.OfferId == offerId))
            return OrderError.OfferAlreadyInOrder;

        var orderItem = new OrderItem(0, Id, offerId, price);
        _orderItems.Add(orderItem);

        UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new OrderItemAddedDomainEvent(Id, offerId));

        return ResultBase.Success();
    }

    public ResultBase RemoveItem(int offerId)
    {
        if (Status.Equals(OrderStatus.Archived) || Status.Equals(OrderStatus.Completed) || Status.Equals(OrderStatus.Cancelled))
            return OrderError.InvalidStatus;

        var item = _orderItems.FirstOrDefault(i => i.OfferId == offerId);
        if (item is null)
            return OrderError.ItemNotFoundInOrder;

        _orderItems.Remove(item);

        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new OrderItemRemovedDomainEvent(Id, offerId));

        return ResultBase.Success();
    }
    public ResultBase MarkAsPaid()
    {
        if (Status.Equals(OrderStatus.Completed))
            return OrderError.AlreadyComplleted;

        if (Status.Equals(OrderStatus.Cancelled))
            return OrderError.InvalidStatus;

        Status = OrderStatus.Completed;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new OrderPaidDomainEvent(Id));
        return ResultBase.Success();
    }
}
