using TMS.Domain.Shared;

namespace TMS.Domain.Commercial.Payment;

public class PaymentStatus : Enumeration<PaymentStatus>
{
    public static readonly PaymentStatus Pending = new PendingStatus();
    public static readonly PaymentStatus Success = new SuccessStatus();
    public static readonly PaymentStatus Failed = new FailedStatus();

    public PaymentStatus(int id, string name) : base(id, name) { }

    private sealed class PendingStatus() : PaymentStatus(1, "Pending") { }
    private sealed class SuccessStatus() : PaymentStatus(2, "Success") { }
    private sealed class FailedStatus() : PaymentStatus(3, "Failed") { }
}
