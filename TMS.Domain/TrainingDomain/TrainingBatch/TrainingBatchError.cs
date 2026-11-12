using TMS.Domain.Shared;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TMS.Domain.Training.TrainingBatch;

public static class TrainingBatchError
{
    public static readonly ErrorBase InvalidCourseID = ErrorBase.Validation(
        "TrainingBatch.InvalidCourseID",
        "The course ID is invalid.");
    public static readonly ErrorBase CourseRequired = ErrorBase.Validation(
        "TrainingBatch.CourseRequired",
        "The course ID is mandaroty.");
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
    public static readonly ErrorBase CannotCreateSessionForThisBatch = ErrorBase.Unauthorized(
        "TrainingBatch.CannotCreateSessionForThisBatch",
        "Cannot Create Session For This Training Batch.");
    public static readonly ErrorBase StartDateRequired = ErrorBase.Validation(
        "Batch.StartDateRequired", 
        "A batch cannot start without a StartDate.");
    public static readonly ErrorBase NoSessionsToStart = ErrorBase.Validation(
        "Batch.NoSessionsToStart", 
        "A batch cannot move to 'In Progress' without at least one session.");
    public static readonly ErrorBase FutureSessionsExist = ErrorBase.Conflict(
        "Batch.FutureSessionsExist", 
        "A batch cannot be completed while there are future sessions.");
}
