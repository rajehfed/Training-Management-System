using TMS.Domain.Shared;

namespace TMS.Domain.Learning.Course;

public static class CourseError
{
    public static readonly ErrorBase InvalidTitle = ErrorBase.Validation(
        "Course.InvalidTitle",
        "The course title is invalid.");
    public static readonly ErrorBase InvalidDescription = ErrorBase.Validation(
        "Course.InvalidDescription",
        "The course description is invalid.");
    public static readonly ErrorBase InvalidDuration = ErrorBase.Validation(
        "Course.InvalidDuration",
        "The course duration is invalid.");
    public static readonly ErrorBase InvalidStatus = ErrorBase.Validation(
        "Course.InvalidStatus",
        "The course status is invalid.");
    public static readonly ErrorBase CourseNotFound = ErrorBase.NotFound(
        "Course.CourseNotFound",
        "The course was not found.");
    public static readonly ErrorBase AlreadyPublished = ErrorBase.Conflict(
        "Course.AlreadyPublished",
        "The course is already published.");
    public static readonly ErrorBase AlreadyArchived = ErrorBase.Conflict(
       "Course.AlreadyArchived",
       "The course is already archived.");
    public static readonly ErrorBase CannotArchiveDraft = ErrorBase.Validation(
        "Course.CannotArchiveDraft",
        "The course cannot be archived while in draft status.");
}