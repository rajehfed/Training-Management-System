using Dapper;
using System.Data;
using TMS.Application.Shared.Interfaces;
using TMS.Application.Shared.Models.DTOs;
using TMS.Domain.LearningDomain.Course;
using TMS.Domain.Training.TrainingBatch;
using TMS.Domain.TrainingDomain.Session;
using TMS.Infrastructure.Dapper;
using TMS.Infrastructure.Mappers;
using static System.Collections.Specialized.BitVector32;

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

    public async Task AddSessionToTrainingBatchAsync(
        Session session,
        CancellationToken cancellationToken = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            INSERT INTO Sessions (
                BatchId,
                StartTime,
                EndTime,
                RoomId,
                Status,
                CreatedAt
            ) VALUES (
                @BatchId,
                @StartTime,
                @EndTime,
                @RoomId,
                @Status,
                @CreatedAt
            );";

        var command = new CommandDefinition(sql, new
        {
            session.BatchId,
            session.StartTime,
            session.EndTime,
            session.RoomId,
            Status = session.Status.Name,
            session.CreatedAt
        }, cancellationToken: cancellationToken);

        await connection.ExecuteAsync(command);
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
            LEFT JOIN Sessions s ON tb.BatchId = s.BatchId;
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
            LEFT JOIN Sessions s ON tb.BatchId = s.BatchId
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

    public async Task<TrainingBatch?> GetByIdWithSessionsAsync(int batchId, CancellationToken cancellationToken = default)
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
            LEFT JOIN Sessions s ON tb.BatchId = s.BatchId
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
            LEFT JOIN Sessions s ON tb.BatchId = s.BatchId
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
            LEFT JOIN Sessions s ON tb.BatchId = s.BatchId
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
            LEFT JOIN Sessions s ON tb.BatchId = s.BatchId
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

    public async Task<bool> SessionExistsByCourseIdInBatchAsync(
        int courseId,
        int sessionId,
        CancellationToken cancellationToken = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT EXISTS(
                SELECT 1
                FROM Trainingbatch tb
                JOIN Sessions s ON tb.BatchId = s.BatchId
                WHERE tb.CourseId = @CourseId AND s.SessionId = @SessionId);";

        var command = new CommandDefinition(
            sql,
            new { CourseId = courseId, SessionId = sessionId },
            cancellationToken: cancellationToken);

        return await connection.ExecuteScalarAsync<bool>(command);
    }

    public async Task<bool> SessionExistsInBatchAsync(
        int batchId,
        int sessionId,
        CancellationToken cancellationToken = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT EXISTS(
                SELECT 1
                FROM Trainingbatch tb
                JOIN Sessions s ON tb.BatchId = s.BatchId
                WHERE tb.BatchId = @BatchId AND s.SessionId = @SessionId);";

        var command = new CommandDefinition(
            sql,
            new { BatchId = batchId, SessionId = sessionId },
            cancellationToken: cancellationToken);

        return await connection.ExecuteScalarAsync<bool>(command);
    }

    public async Task<bool> TrainingBatchExistsAsync(int batchId, CancellationToken cancellationToken = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT EXISTS(
                SELECT 1
                FROM Trainingbatch
                WHERE BatchId = @BatchId);";

        var command = new CommandDefinition(
            sql,
            new { BatchId = batchId },
            cancellationToken: cancellationToken);

        return await connection.ExecuteScalarAsync<bool>(command);
    }

    public async Task<bool> TrainingBatchExistsByCourseIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT EXISTS(
                SELECT 1
                FROM Trainingbatch
                WHERE CourseId = @CourseId);";

        var command = new CommandDefinition(
            sql,
            new { CourseId = courseId },
            cancellationToken: cancellationToken);

        return await connection.ExecuteScalarAsync<bool>(command);
    }

    public async Task UpdateAsync(TrainingBatch trainingBatch, CancellationToken cancellationToken = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
        UPDATE trainingbatch
        SET courseid = @CourseId,
            mentorid = @MentorId,
            startdate = @StartDate,
            enddate = @EndDate,
            status = @Status,
            updatedat = @UpdatedAt
        WHERE batchid = @Id;";

        var command = new CommandDefinition(sql, new
        {
            trainingBatch.Id,
            CourseId = trainingBatch.CourseID,
            MentorId = trainingBatch.MentorID,
            trainingBatch.StartDate,
            trainingBatch.EndDate,
            Status = trainingBatch.Status.Name,
            UpdatedAt = DateTime.UtcNow
        }, cancellationToken: cancellationToken);

        await connection.ExecuteAsync(command);
    }
}
