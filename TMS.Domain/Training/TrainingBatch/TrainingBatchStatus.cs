using TMS.Domain.Shared;

namespace TMS.Domain.Training.TrainingBatch;

public abstract class TrainingBatchStatus : Enumeration<TrainingBatchStatus>
{
    public static readonly TrainingBatchStatus Draft = new DraftStatus();
    public static readonly TrainingBatchStatus EnrollmentOpen = new EnrollmentOpenStatus();
    public static readonly TrainingBatchStatus InProgress = new InProgressStatus();
    public static readonly TrainingBatchStatus Completed = new CompletedStatus();
    public static readonly TrainingBatchStatus Archived = new ArchivedStatus();

    protected TrainingBatchStatus(int id, string name) : base(id, name) { }

    public sealed class DraftStatus() : TrainingBatchStatus(1, "Draft") { }
    public sealed class EnrollmentOpenStatus() : TrainingBatchStatus(2, "Enrollment Open") { }
    public sealed class InProgressStatus() : TrainingBatchStatus(3, "In Progress") { }
    public sealed class CompletedStatus() : TrainingBatchStatus(4, "Completed") { }
    public sealed class ArchivedStatus() : TrainingBatchStatus(5, "Archived") { }
}
