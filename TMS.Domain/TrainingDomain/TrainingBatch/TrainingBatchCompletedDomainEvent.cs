using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.TrainingDomain.TrainingBatch;

public class TrainingBatchCompletedDomainEvent : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
