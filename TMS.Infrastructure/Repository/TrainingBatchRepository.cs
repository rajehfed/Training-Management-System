using Dapper;
using System.Data;
using TMS.Application.Shared.Interfaces;
using TMS.Application.Shared.Models.DTOs;
using TMS.Domain.LearningDomain.Course;
using TMS.Domain.Training.TrainingBatch;
using TMS.Domain.TrainingDomain.Session;
using TMS.Infrastructure.Dapper;
using TMS.Infrastructure.Mappers;

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

    public async Task<List<TrainingBatch>?> GetAllTrainingBatchesAsync(CancellationToken cancellationToken = default)
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
	            tb.UpdatedOn AS UpdatedAt,
                
                s.SessionId,
                s.BatchId,
                s.StartTime,
                s.EndTime,
                s.RoomId,
                s.Status,
                s.CreatedAt,
                s.UpdatedAt
            FROM 
	            Trainingbatch tb
            LEFT JOIN Session s ON tb.BatchId = s.BatchId;
        ";

        var batchesDictionary = new Dictionary<int, TrainingBatchDto>();

        var command = new CommandDefinition(sql, cancellationToken: cancellationToken);

        await connection.QueryAsync<TrainingBatchDto, SessionDto, TrainingBatchDto>(
            command,
            (batch, session) =>
            {
                if (!batchesDictionary.TryGetValue(batch.BatchId, out var currentBatch))
                {
                    currentBatch = batch with { Sessions = new List<SessionDto>() };
                    batchesDictionary.Add(currentBatch.BatchId, currentBatch);
                }

                if (session is not null && !currentBatch.Sessions.Any(s => s.SessionId == session.SessionId))
                    ((List<SessionDto>)currentBatch.Sessions).Add(session);

                return currentBatch;
            }, splitOn: "SessionId");

        return batchesDictionary.Values
            .Select(dto => TrainingBatchMapper.ToDomain(dto))
            .ToList();
    }

    public async Task<TrainingBatch?> GetByIdAsync(int batchId, CancellationToken cancellationToken = default)
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
	            tb.UpdatedOn AS UpdatedAt,
                
                s.SessionId,
                s.BatchId,
                s.StartTime,
                s.EndTime,
                s.RoomId,
                s.Status,
                s.CreatedAt,
                s.UpdatedAt
            FROM 
	            Trainingbatch tb
            LEFT JOIN Session s ON tb.BatchId = s.BatchId
            WHERE tb.BatchId = @BatchId;
        ";

        var command = new CommandDefinition(sql, new { BatchId = batchId }, cancellationToken: cancellationToken);
        var batchDictionary = new Dictionary<int, TrainingBatchDto>();

        await connection.QueryAsync<
            TrainingBatchDto,
            SessionDto,
            TrainingBatchDto>(
                command,
                (batch, session) =>
                {
                    if (!batchDictionary.TryGetValue(batch.BatchId, out var currentBatch))
                    {
                        currentBatch = batch with { Sessions = new List<SessionDto>() };
                        batchDictionary.Add(currentBatch.BatchId, currentBatch);
                    }

                    if (batch is not null && session.SessionId != 0)
                        ((List<SessionDto>)currentBatch.Sessions).Add(session);

                    return currentBatch;
                },
                splitOn: "SessionId"
            );

        var dto = batchDictionary.Values.FirstOrDefault();
        if (dto is null)
            return null;

        return TrainingBatchMapper.ToDomain(dto);
    }

    public Task<TrainingBatch?> GetByIdWithSessionsAsync(int batchId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TrainingBatch>?> GetTrainingBatchesByCourseIdAsync(int courseId, CancellationToken cancellationToken = default)
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
	            tb.UpdatedOn AS UpdatedAt,
                
                s.SessionId,
                s.BatchId,
                s.StartTime,
                s.EndTime,
                s.RoomId,
                s.Status,
                s.CreatedAt,
                s.UpdatedAt
            FROM 
	            Trainingbatch tb
            LEFT JOIN Session s ON tb.BatchId = s.BatchId
            WHERE tb.CourseId = @CourseId;
        ";

        var batchesDictionary = new Dictionary<int, TrainingBatchDto>();

        var command = new CommandDefinition(sql, new { CourseId = courseId }, cancellationToken: cancellationToken);

        await connection.QueryAsync<TrainingBatchDto, SessionDto, TrainingBatchDto>(
            command,
            (batch, session) =>
            {
                if (!batchesDictionary.TryGetValue(batch.BatchId, out var currentBatch))
                {
                    currentBatch = batch with { Sessions = new List<SessionDto>() };
                    batchesDictionary.Add(currentBatch.BatchId, currentBatch);
                }

                if (session is not null && !currentBatch.Sessions.Any(s => s.SessionId == session.SessionId))
                    ((List<SessionDto>)currentBatch.Sessions).Add(session);

                return currentBatch;
            }, splitOn: "SessionId");

        return batchesDictionary.Values
            .Select(dto => TrainingBatchMapper.ToDomain(dto))
            .ToList();
    }

    public async Task<List<TrainingBatch>?> GetTrainingBatchesByMentorIdAsync(int mentorId, CancellationToken cancellationToken = default)
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
	            tb.UpdatedOn AS UpdatedAt,
                
                s.SessionId,
                s.BatchId,
                s.StartTime,
                s.EndTime,
                s.RoomId,
                s.Status,
                s.CreatedAt,
                s.UpdatedAt
            FROM 
	            Trainingbatch tb
            LEFT JOIN Session s ON tb.BatchId = s.BatchId
            WHERE tb.MentorId = @MentorId;
        ";

        var batchesDictionary = new Dictionary<int, TrainingBatchDto>();

        var command = new CommandDefinition(sql, new { MentorId = mentorId }, cancellationToken: cancellationToken);

        await connection.QueryAsync<TrainingBatchDto, SessionDto, TrainingBatchDto>(
            command,
            (batch, session) =>
            {
                if (!batchesDictionary.TryGetValue(batch.BatchId, out var currentBatch))
                {
                    currentBatch = batch with { Sessions = new List<SessionDto>() };
                    batchesDictionary.Add(currentBatch.BatchId, currentBatch);
                }

                if (session is not null && !currentBatch.Sessions.Any(s => s.SessionId == session.SessionId))
                    ((List<SessionDto>)currentBatch.Sessions).Add(session);

                return currentBatch;
            }, splitOn: "SessionId");

        return batchesDictionary.Values
            .Select(dto => TrainingBatchMapper.ToDomain(dto))
            .ToList();
    }

    public async Task<List<TrainingBatch>?> GetTrainingBatchesByStatusAsync(
        TrainingBatchStatus status,
        CancellationToken cancellationToken = default)
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
                tb.UpdatedOn AS UpdatedAt,
        
                s.SessionId,
                s.BatchId,
                s.StartTime,
                s.EndTime,
                s.RoomId,
                s.Status,
                s.CreatedAt,
                s.UpdatedAt
            FROM 
                Trainingbatch tb
            LEFT JOIN Session s ON tb.BatchId = s.BatchId
            WHERE tb.Status = @Status;
        ";

        var batchesDictionary = new Dictionary<int, TrainingBatchDto>();

        var command = new CommandDefinition(sql, new { Status = status.Name }, cancellationToken: cancellationToken);

        await connection.QueryAsync<TrainingBatchDto, SessionDto, TrainingBatchDto>(
            command,
            (batch, session) =>
            {
                if (!batchesDictionary.TryGetValue(batch.BatchId, out var currentBatch))
                {
                    currentBatch = batch with { Sessions = new List<SessionDto>() };
                    batchesDictionary.Add(currentBatch.BatchId, currentBatch);
                }

                if (session is not null && !currentBatch.Sessions.Any(s => s.SessionId == session.SessionId))
                    ((List<SessionDto>)currentBatch.Sessions).Add(session);

                return currentBatch;
            }, splitOn: "SessionId");

        return batchesDictionary.Values
            .Select(dto => TrainingBatchMapper.ToDomain(dto))
            .ToList();
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
