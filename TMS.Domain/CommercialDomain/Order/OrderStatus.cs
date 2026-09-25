using TMS.Domain.Shared;

namespace TMS.Domain.Commercial.Order;

public abstract class OrderStatus : Enumeration<OrderStatus>
{
    public static readonly OrderStatus Draft = new DraftStatus();
    public static readonly OrderStatus PendingPayment = new PendingPaymentStatus();
    public static readonly OrderStatus Completed = new CompletedStatus();
    public static readonly OrderStatus Cancelled = new CancelledStatus();
    public static readonly OrderStatus Archived = new ArchivedStatus();

    protected OrderStatus(int id, string name) : base(id, name) { }

    private sealed class DraftStatus() : OrderStatus(1, "Draft") { }
    private sealed class PendingPaymentStatus() : OrderStatus(2, "Pending Payment") { }
    private sealed class CompletedStatus() : OrderStatus(3, "Completed") { }
    private sealed class CancelledStatus() : OrderStatus(4, "Cancelled") { }
    private sealed class ArchivedStatus() : OrderStatus(5, "Archived") { }
}
