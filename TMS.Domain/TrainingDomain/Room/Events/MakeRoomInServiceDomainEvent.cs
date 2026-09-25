using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.Training.Room.Events;

public record MakeRoomInServiceDomainEvent(int RoomId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
