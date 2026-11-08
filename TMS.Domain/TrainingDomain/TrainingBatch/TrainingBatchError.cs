using TMS.Domain.Shared;

namespace TMS.Domain.Training.TrainingBatch;

public static class TrainingBatchError
{
    public static readonly ErrorBase InvalidCourseID = ErrorBase.Validation(
        "TrainingBatch.InvalidCourseID",
        "The course ID is invalid.");
    public static readonly ErrorBase InvalidMentorID = ErrorBase.Validation(
        "TrainingBatch.InvalidMentorID",
        "The mentor ID is invalid.");
    public static readonly ErrorBase InvalidStartDate = ErrorBase.Validation(
        "TrainingBatch.InvalidStartDate",
        "The start date is invalid.");
    public static readonly ErrorBase InvalidEndDate = ErrorBase.Validation(
        "TrainingBatch.InvalidEndDate",
        "The end date is invalid.");
    public static readonly ErrorBase TrainingBatchNotFound = ErrorBase.NotFound(
        "TrainingBatch.TrainingBatchNotFound",
        "The training batch was not found.");
    public static readonly ErrorBase TrainingBatchAlreadyPointsToACourse = ErrorBase.Conflict(
        "TrainingBatch.TrainingBatchAlreadyPointsToACourse",
        "The training batch already points to a course.");
}
