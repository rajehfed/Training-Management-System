using TMS.Domain.Shared;

namespace TMS.Domain.Commercial.Payment;

public static class PaymentError
{
    public static readonly ErrorBase AlreadySucceeded = ErrorBase.Conflict(
        "Payment.Already",
        "The status of the payment is already success or completed");
    public static readonly ErrorBase CannotCompleteFailedPayment = ErrorBase.Forbidden(
        "Payment.CannotCompleteFailedPayment",
        "Cannot Complete Failed Payment");
    public static readonly ErrorBase InvalidTransactionReference = ErrorBase.Validation(
        "Payment.InvalidTransactionReference",
        "The Transaction Reference is not valid to perform an action");
    public static readonly ErrorBase CannotFailCompletedPayment = ErrorBase.Conflict(
        "Payment.CannotFailCompletedPayment",
        "Cannot Fail CompletedPayment");
}
