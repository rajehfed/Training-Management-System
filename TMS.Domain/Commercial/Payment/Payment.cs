using TMS.Domain.Commercial.Payment.Events;
using TMS.Domain.Shared;
using TMS.Domain.Shared.Results;

namespace TMS.Domain.Commercial.Payment;

public class Payment : AggregateRoot<int>
{
    public int OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;
    public string? TransactionReference { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public Payment() : base(default) { }
    public Payment(
        int paymentId,
        int orderId,
        decimal amount) : base(paymentId)
    {
        if (orderId <= 0)
            throw new ArgumentOutOfRangeException(nameof(orderId), "OrderId must be greater than zero.");

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");

        OrderId = orderId;
        Amount = amount;
        Status = PaymentStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public ResultBase CompletePayment(string transactionReference)
    {
        if (this.Status.Equals(PaymentStatus.Success))
            return PaymentError.AlreadySucceeded;

        if (this.Status.Equals(PaymentStatus.Failed))
            return PaymentError.CannotCompleteFailedPayment;

        if (string.IsNullOrWhiteSpace(transactionReference))
            return PaymentError.InvalidTransactionReference;

        Status = PaymentStatus.Success;
        TransactionReference = transactionReference;
        CompletedAt = DateTime.UtcNow;

        this.AddDomainEvent(new PaymentSucceededDomainEvent(Id, OrderId, Amount));
        return ResultBase.Success();
    }
    public ResultBase MarkAsFailed(string failureReason)
    {
        if (Status.Equals(PaymentStatus.Success))
            return PaymentError.CannotFailCompletedPayment;

        Status = PaymentStatus.Failed;
        CompletedAt = DateTime.UtcNow;

        AddDomainEvent(new PaymentFailedDomainEvent(Id, OrderId, failureReason));

        return ResultBase.Success();
    }
}
