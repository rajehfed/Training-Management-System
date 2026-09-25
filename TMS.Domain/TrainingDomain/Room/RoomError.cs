using TMS.Domain.Shared;

namespace TMS.Domain.Training.Room;

public static class RoomError
{
    public static readonly ErrorBase AlreadySuspend = ErrorBase.Conflict(
        "Room.AlreadySuspend",
        "The status of the room is already suspended.");
    public static readonly ErrorBase InvalidStatus = ErrorBase.Validation(
        "Room.InvalidStatus",
        "The status of the room is not valid.");
    public static readonly ErrorBase InvalidCapacity = ErrorBase.Validation(
        "Room.InvalidCapacity",
        "The Capacity is not valid.");
    public static readonly ErrorBase InvalidRoomNumber = ErrorBase.Validation(
        "Room.InvalidRoomNumber",
        "The room number is invalid.");
}
