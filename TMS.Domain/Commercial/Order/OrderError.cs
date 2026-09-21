using TMS.Domain.Shared;

namespace TMS.Domain.Commercial.Order;

public static class OrderError
{
    public static readonly ErrorBase InvalidStatus = ErrorBase.Validation(
        "Order.InvalidStatus",
        "The order status is not valid");
    public static readonly ErrorBase AlreadyCancelled = ErrorBase.Conflict(
        "Order.AlreadyCancelled",
        "The Order is already Cancelled");
    public static readonly ErrorBase AlreadyArchived = ErrorBase.Conflict(
        "Order.AlreadyArchived",
        "The Order is already Archived");
    public static readonly ErrorBase PaymentNotCompleted = ErrorBase.Validation(
        "Order.PaymentNotCompleted",
        "The payment is not completed");
    public static readonly ErrorBase PaymentFailed = ErrorBase.Validation(
        "Order.PaymentFailed",
        "The payment is failed");
    public static readonly ErrorBase AlreadyComplleted = ErrorBase.Validation(
        "Order.AlreadyComplleted",
        "The Order is already complleted");
}
