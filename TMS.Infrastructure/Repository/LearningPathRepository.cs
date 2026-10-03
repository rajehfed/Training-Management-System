using Dapper;
using TMS.Application.Shared.Interfaces;
using TMS.Domain.Commercial.SellableItem;
using TMS.Domain.LearningDomain.Course;
using TMS.Domain.LearningDomain.LearningPath;
using TMS.Infrastructure.Dapper;

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
            WHERE learningpath.sellableitemid = @PathId";

        var command = new CommandDefinition(sql, new { PathId = pathId }, cancellationToken: cancellationToken);

        return await connection.QueryFirstOrDefaultAsync<LearningPath?>(command);
    }

    public Task<LearningPath?> GetByNameAsync(string pathName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Update(LearningPath learningPath)
    {
        throw new NotImplementedException();
    }
}
