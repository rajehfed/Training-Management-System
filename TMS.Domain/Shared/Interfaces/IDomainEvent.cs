using MediatR;

namespace TMS.Domain.Shared.Interfaces;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
