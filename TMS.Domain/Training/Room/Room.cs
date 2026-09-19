using TMS.Domain.Shared;
using TMS.Domain.Shared.Results;
using TMS.Domain.Training.Room.Events;

namespace TMS.Domain.Training.Room;

public class Room : AggregateRoot<int>
{
    public string RoomNumber { get; private set; } = null!;
    public RoomCapacity Capacity { get; private set; } = null!;
    public RoomStatus Status { get; private set; } = RoomStatus.Pending;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    protected Room() : base(default) { }
    public Room(int roomId, string roomNumber, RoomCapacity capacity) : base(roomId)
    {
        if (string.IsNullOrEmpty(roomNumber))
            throw new ArgumentException("The room number must be not null", nameof(roomNumber));

        this.RoomNumber = roomNumber;
        this.Capacity = capacity;
        this.Status = RoomStatus.Pending;
        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = null;
    }

    public ResultBase Suspend()
    {
        if (this.Status == RoomStatus.Suspended)
            return RoomError.AlreadySuspend;

        this.Status = RoomStatus.Suspended;
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new MakeRoomSuspended(this.Id));

        return ResultBase.Success();
    }
    public ResultBase InService()
    {
        if (this.Status == RoomStatus.Suspended)
            return RoomError.InvalidStatus;

        this.Status = RoomStatus.InService;
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new MakeRoomInServiceDomainEvent(this.Id));

        return ResultBase.Success();
    }
    public ResultBase Pending()
    {
        if (this.Status == RoomStatus.Suspended)
            return RoomError.InvalidStatus;

        this.Status = RoomStatus.Pending;
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new MakeRoomPendingDomainEvent(this.Id));

        return ResultBase.Success();
    }

    public ResultBase ChangeRoomNumber(string roomNumber)
    {
        if (string.IsNullOrEmpty(roomNumber))
            return RoomError.InvalidRoomNumber;

        this.RoomNumber = roomNumber;
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new UpdateRoomDomainEvent(this.Id));

        return ResultBase.Success();
    }
    public ResultBase ChangeRoomCapacity(int capacity)
    {
        this.Capacity = RoomCapacity.FromCapacity(capacity);
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new UpdateRoomDomainEvent(this.Id));

        return ResultBase.Success();
    }
}
