using Dapper;
using System.Data;
using TMS.Application.Shared.Interfaces;
using TMS.Domain.Training.TrainingBatch;
using TMS.Infrastructure.Dapper;

namespace TMS.Infrastructure.Repository;

public class TrainingBatchRepository : ITrainingBatchRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public TrainingBatchRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    public async Task AddAsync(TrainingBatch trainingBatch, CancellationToken cancellationToken = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        connection.Open();

        const string SqlDbType = @"
                INSERT INTO public.trainingbatch(
	                courseId, 
	                mentorId, 
	                startDate, 
	                endDate, 
	                status, 
	                createdOn, 
	                updatedOn
                ) VALUES (
	                @courseId, 
	                @mentorId, 
	                @startDate, 
	                @endDate, 
	                @status, 
	                @createdOn, 
	                @updatedOn
                );";

        var command = new CommandDefinition(
            SqlDbType,
            new
            {
                courseId = trainingBatch.CourseID,
                mentorId = trainingBatch.MentorID,
                startDate = trainingBatch.StartDate,
                endDate = trainingBatch.EndDate,
                status = trainingBatch.Status.Name,
                createdOn = trainingBatch.CreatedAt,
                updatedOn = trainingBatch.UpdatedAt,
            },
            commandType: CommandType.Text);

        await connection.ExecuteAsync(command);
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
        using var connection = _dbConnectionFactory.CreateConnection();
        connection.Open();

        const string sql = @"
            SELECT 
	            tb.BatchId AS Id, 
	            tb.CourseId AS CourseID, 
                
	            tb.MentorId AS MentorID, 
	            tb.StartDate AS StartDate, 
	            tb.EndDate AS EndDate, 
	            tb.Status AS Status, 
	            tb.CreatedOn AS CreatedAt, 
	            tb.UpdatedOn AS UpdatedAt
            FROM 
	            Trainingbatch tb
            LEFT JOIN Session s ON tb.BatchId = s.BatchId
            LEFT JOIN Course c ON tb.CourseId = c.CourseId
            ;
        ";

        return [];
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
