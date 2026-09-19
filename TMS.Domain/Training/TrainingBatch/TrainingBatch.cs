using TMS.Domain.Shared;

namespace TMS.Domain.Training.TrainingBatch;

public class TrainingBatch : AggregateRoot<int>
{
    public int CourseID { get; private set; }
    public int? MentorID { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public TrainingBatchStatus Status { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public TrainingBatch() : base(default) { }
    public TrainingBatch(
        int id,
        int courseId,
        int? mentorId,
        DateTime startDate,
        DateTime? endDate) : base(id)
    {
        CourseID = courseId;
        MentorID = mentorId;
        StartDate = startDate;
        EndDate = endDate;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
        Status = TrainingBatchStatus.Draft;
    }

    
}
