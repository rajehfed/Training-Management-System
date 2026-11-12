using TMS.Domain.Shared;
using TMS.Domain.Shared.Results;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TMS.Domain.TrainingDomain.Session;

public class Session : Entity<int>
{
    public int BatchId { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public int? RoomId { get; private set; }
    public SessionStatus Status { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected Session(int id) : base(id) { }
    protected Session(
        int id,
        int batchId,
        DateTime startTime,
        DateTime endTime,
        int? roomId,
        SessionStatus status,
        DateTime createdAt,
        DateTime? updatedAt) : base(id)
    {
        if(startTime >= endTime)
            throw new DomainException(SessionError.InvalidTimeRange);

        BatchId = batchId;
        StartTime = startTime;
        EndTime = endTime;
        RoomId = roomId;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
    public static Session Create(
        int batchId,
        DateTime startTime,
        DateTime endTime,
        int? roomId)
    {
        return new Session(
            id: 0,
            batchId: batchId,
            startTime: startTime,
            endTime: endTime,
            roomId: roomId,
            status: SessionStatus.Scheduled,
            createdAt: DateTime.UtcNow,
            updatedAt: null);
    }

    public ResultBase Start()
    {
        if (!this.Status.Equals(SessionStatus.Scheduled))
            return SessionError.CannotStartBeforeScheduled;

        this.Status = SessionStatus.InProgress;
        this.UpdatedAt = DateTime.UtcNow;

        return ResultBase.Success();
    }
    public ResultBase Completed()
    {
        if (this.Status.Equals(SessionStatus.Completed))
            return SessionError.AlreadyCompleted;

        Status = SessionStatus.Completed;
        UpdatedAt = DateTime.UtcNow;

        return ResultBase.Success();
    }

    public ResultBase Cancel()
    {
        if (Status == SessionStatus.Completed)
            return SessionError.AlreadyCompleted;

        Status = SessionStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;

        return ResultBase.Success();
    }

    public ResultBase ValidateAttendanceCanBeRecorded()
    {
        if (Status.Equals(SessionStatus.Scheduled))
            return SessionError.AttendanceNotAllowedYet;

        if (Status.Equals(SessionStatus.Cancelled))
            return SessionError.SessionCancelledAttendanceNotAllowed;

        return ResultBase.Success();
    }
}
