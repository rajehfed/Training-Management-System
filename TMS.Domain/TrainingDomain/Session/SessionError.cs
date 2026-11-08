using TMS.Domain.Shared;

namespace TMS.Domain.TrainingDomain.Session;

public static class SessionError
{
    public static readonly ErrorBase InvalidTimeRange = ErrorBase.Conflict(
        "Session.InvalidTimeRange",
        "The EndTime must be strictly after the StartTime.");

    public static readonly ErrorBase AlreadyCompleted = ErrorBase.Conflict(
        "Session.AlreadyCompleted",
        "A completed session cannot be rescheduled or modified.");

    public static readonly ErrorBase AlreadyCancelled = ErrorBase.Conflict(
        "Session.AlreadyCancelled",
        "This session has already been cancelled.");

    public static readonly ErrorBase CannotStartBeforeScheduled = ErrorBase.Forbidden(
        "Session.CannotStartBeforeScheduled",
        "Only scheduled sessions can be marked as in progress.");

    public static readonly ErrorBase AttendanceNotAllowedYet = ErrorBase.Forbidden(
        "Session.AttendanceNotAllowedYet",
        "Attendance can only be recorded after the session has started.");
}
