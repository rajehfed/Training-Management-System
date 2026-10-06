using Dapper;
using TMS.Application.Shared.Interfaces;
using TMS.Application.Shared.Models.DTOs;
using TMS.Domain.LearningDomain.LearningPath;
using TMS.Infrastructure.Dapper;
using TMS.Infrastructure.Mappers;

namespace TMS.Infrastructure.Repository;

public class LearningPathRepository : ILearningPathRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public LearningPathRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task AddAsync(LearningPath learningPath, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            const string sqlSellableItem = @"
                INSERT INTO SellableItem
                    (ItemType, CreatedAt)
                VALUES
                    (@ItemType, @CreatedAt)
                RETURNING SellableItemId; ";

            var sellableItemId = await connection.ExecuteScalarAsync<int>(
                new CommandDefinition(
                    sqlSellableItem,
                    new { ItemType = "LearningPath", learningPath.CreatedAt },
                    transaction: transaction,
                    cancellationToken: cancellationToken)
                );

            const string sqlLearningPath = @"
                INSERT INTO 
                public.learningpath(
                    SellableItemId, 
                    PathName, 
                    Outcomes, 
                    Audience, 
                    Status, 
                    CreatedAt, 
                    UpdatedAt)
	            VALUES (
                    @SellableItemId, 
                    @PathName, 
                    @Outcomes, 
                    @Audience, 
                    @Status, 
                    @CreatedAt, 
                    @UpdatedAt);
            ";

            var learningPathCommand = new CommandDefinition(
                sqlLearningPath,
                new
                {
                    SellableItemId = sellableItemId,
                    learningPath.PathName,
                    learningPath.Outcomes,
                    learningPath.Audience,
                    learningPath.Status.Name,
                    learningPath.CreatedAt,
                    learningPath.UpdatedAt
                },
                transaction: transaction,
                cancellationToken: cancellationToken);

            await connection.ExecuteAsync(learningPathCommand);
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<bool> CourseExistsInPath(int pathId, int courseId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
        SELECT EXISTS(
            SELECT 1 
            FROM courseinpath 
            where 
                courseinpath.courseid = @CourseId AND 
                courseinpath.learningpathid = @PathId)";

        var command = new CommandDefinition(sql, new { CourseId = courseId, PathId = pathId }, cancellationToken: cancellationToken);

        return await connection.ExecuteScalarAsync<bool>(command);
    }

    public void Delete(LearningPath learningPath)
    {
        // I'll Do it letter, but for now, I will throw an exception to indicate that this method is not yet implemented.
        throw new NotImplementedException();
    }

    public async Task<LearningPath?> GetByIdAsync(int pathId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            SELECT 
                LearningPathId AS Id, 
                SellableItemId, 
                PathName, 
                Outcomes, 
                Audience, 
                Status, 
                CreatedAt, 
                UpdatedAt
            FROM learningpath
            WHERE learningpath.LearningPathId = @PathId";

        var command = new CommandDefinition(sql, new { PathId = pathId }, cancellationToken: cancellationToken);

        var dto = await connection.QueryFirstOrDefaultAsync<LearningPathDto?>(command);
        if (dto is null) return null;

        return LearningPathMapper.ToDomain(dto);
    }

    public async Task<LearningPath?> GetByNameAsync(string pathName, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            SELECT 
                LearningPathId AS Id, 
                SellableItemId, 
                PathName, 
                Outcomes, 
                Audience, 
                Status, 
                CreatedAt, 
                UpdatedAt
            FROM learningpath
            WHERE learningpath.PathName = @PathName";

        var command = new CommandDefinition(sql, new { PathName = pathName }, cancellationToken: cancellationToken);

        var dto = await connection.QueryFirstOrDefaultAsync<LearningPathDto?>(command);
        if (dto is null) return null;

        return LearningPathMapper.ToDomain(dto);
    }

    public void Update(LearningPath learningPath)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            UPDATE learningpath
            SET 
                PathName = @PathName,
                SellableItemId = @SellableItemId,
                Outcomes = @Outcomes,
                Audience = @Audience,
                Status = @Status,
                UpdatedAt = @UpdatedAt
            WHERE LearningPathId = @PathId";

        var command = new CommandDefinition(sql, new
        {
            PathId = learningPath.Id,
            learningPath.PathName,
            learningPath.SellableItemId,
            learningPath.Outcomes,
            learningPath.Audience,
            Status = learningPath.Status.Name,
            UpdatedAt = learningPath.UpdatedAt
        });

        connection.Execute(command);
    }
}
