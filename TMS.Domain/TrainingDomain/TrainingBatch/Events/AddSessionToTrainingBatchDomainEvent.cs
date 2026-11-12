using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.TrainingDomain.TrainingBatch.Events;

public class AddSessionToTrainingBatchDomainEvent : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
