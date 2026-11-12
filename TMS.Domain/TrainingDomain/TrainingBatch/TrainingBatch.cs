using TMS.Domain.LearningDomain.LearningPath;
using TMS.Domain.Shared;
using TMS.Domain.Shared.Results;
using TMS.Domain.TrainingDomain.Session;
using TMS.Domain.TrainingDomain.TrainingBatch;
using TMS.Domain.TrainingDomain.TrainingBatch.Events;

namespace TMS.Domain.Training.TrainingBatch;

public class TrainingBatch : AggregateRoot<int>
{
    public int CourseID { get; private set; }
    public int? MentorID { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public TrainingBatchStatus Status { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private readonly List<Session> _sessions = new List<Session>();
    public IReadOnlyCollection<Session> Sessions => _sessions.AsReadOnly();
    public TrainingBatch() : base(default) { }
    public TrainingBatch(
        int id,
        int courseId,
        int? mentorId,
        DateTime startDate,
        DateTime? endDate,
        TrainingBatchStatus status,
        DateTime createdAt,
        DateTime? updatedAt) : base(id)
    {
        if (courseId <= 0)
            throw new DomainException(TrainingBatchError.CourseRequired);

        CourseID = courseId;
        MentorID = mentorId;
        StartDate = startDate;
        EndDate = endDate;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Status = status;
    }

    public TrainingBatch Create(
        int courseId,
        int mentorId,
        DateTime startDate,
        DateTime endDate) => new(
            id: 0,
            courseId: courseId,
            mentorId: mentorId,
            startDate: startDate,
            endDate: endDate,
            status: TrainingBatchStatus.Draft,
            createdAt: DateTime.UtcNow,
            updatedAt: null);

    public ResultBase AddSession(DateTime startTime, DateTime endTime, int? roomID)
    {
        var sessionResult = Session.Create(this.Id, startTime, endTime, roomID);

        if (sessionResult is null)
            return TrainingBatchError.CannotCreateSessionForThisBatch;


        this.AddDomainEvent(new AddSessionToTrainingBatchDomainEvent());
        _sessions.Add(sessionResult);
        
        this.UpdatedAt = DateTime.UtcNow;
        return ResultBase.Success();
    }

    public ResultBase Start()
    {
        if (!StartDate.HasValue)
            return TrainingBatchError.StartDateRequired;

        if (_sessions.Count == 0)
            return TrainingBatchError.NoSessionsToStart;

        this.AddDomainEvent(new TrainingBatchStartedDomainEvent());
        Status = TrainingBatchStatus.InProgress;

        UpdatedAt = DateTime.UtcNow;
        return ResultBase.Success();
    }
    public ResultBase Complete()
    {
        var hasFutureSessions = _sessions.Any(s => s.StartTime > DateTime.UtcNow);

        if (hasFutureSessions)
            return TrainingBatchError.FutureSessionsExist;

        this.AddDomainEvent(new TrainingBatchCompletedDomainEvent());
        Status = TrainingBatchStatus.Completed;

        UpdatedAt = DateTime.UtcNow;
        return ResultBase.Success();
    }
}
