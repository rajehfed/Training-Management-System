using TMS.Domain.Shared;

namespace TMS.Domain.TrainingDomain.Session;

public abstract class SessionStatus : Enumeration<SessionStatus>
{
    public static readonly SessionStatus Scheduled = new ScheduledStatus();
    public static readonly SessionStatus InProgress = new InProgressStatus();
    public static readonly SessionStatus Completed = new CompletedStatus();
    public static readonly SessionStatus Cancelled = new CancelledStatus();

    protected SessionStatus(int id, string name) : base(id, name) { }

    public sealed class ScheduledStatus() : SessionStatus(1, "Scheduled");
    public sealed class InProgressStatus() : SessionStatus(2, "In Progress");
    public sealed class CompletedStatus() : SessionStatus(3, "Completed");
    public sealed class CancelledStatus() : SessionStatus(4, "Cancelled");
}
