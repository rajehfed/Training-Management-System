using TMS.Domain.Shared;

namespace TMS.Domain.Training.Room;

public abstract class RoomStatus : Enumeration<RoomStatus>
{
    public static readonly RoomStatus Pending = new PendingStatus();
    public static readonly RoomStatus InService = new InServiceStatus();
    public static readonly RoomStatus Suspended = new SuspendedStatus();
    public RoomStatus(int id, string name) : base(id, name) { }

    public sealed class PendingStatus() : RoomStatus(1, "Pending") { }
    public sealed class InServiceStatus() : RoomStatus(2, "InService");
    public sealed class SuspendedStatus() : RoomStatus(3, "Suspended");
}
