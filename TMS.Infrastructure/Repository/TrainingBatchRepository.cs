using TMS.Application.Shared.Interfaces;
using TMS.Domain.Training.TrainingBatch;

namespace TMS.Infrastructure.Repository;

public class TrainingBatchRepository : ITrainingBatchRepository
{
    public Task AddAsync(TrainingBatch trainingBatch, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task AddSessionToTrainingBatchAsync(
        int batchId,
        DateTime startTime,
        DateTime endTime,
        int? roomID,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(TrainingBatch trainingBatch, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<TrainingBatch>?> GetAllTrainingBatchesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TrainingBatch?> GetByIdAsync(int batchId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TrainingBatch?> GetByIdWithSessionsAsync(int batchId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<TrainingBatch>?> GetTrainingBatchesByCourseIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<TrainingBatch>?> GetTrainingBatchesByMentorIdAsync(int mentorId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<TrainingBatch>?> GetTrainingBatchesByStatusAsync(
        TrainingBatchStatus status,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task RemoveSessionFromTrainingBatchAsync(
        int batchId,
        int sessionId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SessionExistsByCourseIdInBatchAsync(
        int courseId,
        int sessionId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SessionExistsInBatchAsync(
        int batchId,
        int sessionId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> TrainingBatchExistsAsync(int batchId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> TrainingBatchExistsByCourseIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(TrainingBatch trainingBatch, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
