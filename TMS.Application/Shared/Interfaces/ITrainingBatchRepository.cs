using TMS.Domain.Training.TrainingBatch;

namespace TMS.Application.Shared.Interfaces;

public interface ITrainingBatchRepository
{
    Task<TrainingBatch?> GetByIdAsync(int batchId, CancellationToken cancellationToken = default);
    Task<TrainingBatch?> GetByIdWithSessionsAsync(int batchId, CancellationToken cancellationToken = default);
    Task AddAsync(TrainingBatch trainingBatch, CancellationToken cancellationToken = default);
    Task UpdateAsync(TrainingBatch trainingBatch, CancellationToken cancellationToken = default);
    Task DeleteAsync(TrainingBatch trainingBatch, CancellationToken cancellationToken = default);
    Task<List<TrainingBatch>?> GetAllTrainingBatchesAsync(CancellationToken cancellationToken = default);
    Task<List<TrainingBatch>?> GetTrainingBatchesByCourseIdAsync(int courseId, CancellationToken cancellationToken = default);
    Task<List<TrainingBatch>?> GetTrainingBatchesByMentorIdAsync(int mentorId, CancellationToken cancellationToken = default);
    Task<List<TrainingBatch>?> GetTrainingBatchesByStatusAsync(TrainingBatchStatus status, CancellationToken cancellationToken = default);
    Task AddSessionToTrainingBatchAsync(int batchId, DateTime startTime, DateTime endTime, int? roomID, CancellationToken cancellationToken = default);
    Task RemoveSessionFromTrainingBatchAsync(int batchId, int sessionId, CancellationToken cancellationToken = default);
    Task<bool> SessionExistsInBatchAsync(int batchId, int sessionId, CancellationToken cancellationToken = default);
    Task<bool> TrainingBatchExistsAsync(int batchId, CancellationToken cancellationToken = default);
    Task<bool> TrainingBatchExistsByCourseIdAsync(int courseId, CancellationToken cancellationToken = default);
    Task<bool> SessionExistsByCourseIdInBatchAsync(int courseId, int sessionId, CancellationToken cancellationToken = default);
}
