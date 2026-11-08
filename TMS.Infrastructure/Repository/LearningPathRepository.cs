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
                    (SellableItemtype, CreatedAt)
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
                    Outcomes = learningPath.Outcomes?.Value,
                    learningPath.Audience,
                    Status = learningPath.Status.Name,
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
	            lp.LearningPathId AS PathId, 
	            lp.SellableItemId, 
	            lp.PathName, 
	            lp.Outcomes, 
	            lp.Audience, 
	            lp.Status, 
	            lp.CreatedAt, 
	            lp.UpdatedAt,
	            cip.CourseId,
	            c.CourseName,
	            cip.OrderInPath,
	            c.Description
            FROM learningpath lp
            LEFT JOIN courseinpath cip ON cip.LearningPathId = lp.LearningPathId
            LEFT JOIN course c ON cip.CourseId = c.CourseId
            WHERE lp.LearningPathId = @PathId;";

        var command = new CommandDefinition(sql, new { PathId = pathId }, cancellationToken: cancellationToken);
        var pathDictionary = new Dictionary<int, LearningPathDto>();

        await connection.QueryAsync<
            LearningPathDto, 
            CourseInPathDto, 
            LearningPathDto> (
            command,
            (path, course) =>
            {
                if (!pathDictionary.TryGetValue(path.PathId, out var currentPath))
                {
                    currentPath = path with { Courses = new List<CourseInPathDto>() };
                    pathDictionary.Add(currentPath.PathId, currentPath);
                }

                if (course is not null && course.CourseId != 0)
                    ((List<CourseInPathDto>)currentPath.Courses).Add(course);

                return currentPath;
            },
            splitOn:"CourseId"
            );

        var dto = pathDictionary.Values.FirstOrDefault();
        if (dto is null) return null;

        return LearningPathMapper.ToDomain(dto);
    }

    public async Task<LearningPath?> GetByNameAsync(string pathName, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            SELECT 
	            lp.LearningPathId AS PathId, 
	            lp.SellableItemId, 
	            lp.PathName, 
	            lp.Outcomes, 
	            lp.Audience, 
	            lp.Status, 
	            lp.CreatedAt, 
	            lp.UpdatedAt,
	            cip.CourseId,
	            c.CourseName,
	            cip.OrderInPath,
	            c.Description
            FROM learningpath lp
            LEFT JOIN courseinpath cip ON cip.LearningPathId = lp.LearningPathId
            LEFT JOIN course c ON cip.CourseId = c.CourseId
            WHERE lp.PathName = @pathName;";

        var command = new CommandDefinition(sql, new { PathName = pathName }, cancellationToken: cancellationToken);
        var pathDictionary = new Dictionary<int, LearningPathDto>();

        await connection.QueryAsync<
            LearningPathDto,
            CourseInPathDto,
            LearningPathDto>(
            command,
            (path, course) =>
            {
                if (!pathDictionary.TryGetValue(path.PathId, out var currentPath))
                {
                    currentPath = path with { Courses = new List<CourseInPathDto>() };
                    pathDictionary.Add(currentPath.PathId, currentPath);
                }

                if (course is not null && course.CourseId != 0)
                    ((List<CourseInPathDto>)currentPath.Courses).Add(course);

                return currentPath;
            },
            splitOn: "CourseId"
            );

        var dto = pathDictionary.Values.FirstOrDefault();
        if (dto is null) return null;

        return LearningPathMapper.ToDomain(dto);
    }
    public async Task<List<LearningPath>?> GetAllLearningPathsAsync(CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            SELECT 
	            lp.LearningPathId AS PathId, 
	            lp.SellableItemId, 
	            lp.PathName, 
	            lp.Outcomes, 
	            lp.Audience, 
	            lp.Status, 
	            lp.CreatedAt, 
	            lp.UpdatedAt,
	            cip.CourseId,
	            c.CourseName,
	            cip.OrderInPath,
	            c.Description
            FROM learningpath lp
            LEFT JOIN courseinpath cip ON cip.LearningPathId = lp.LearningPathId
            LEFT JOIN course c ON cip.CourseId = c.CourseId;";

        var learningPathDictionary = new Dictionary<int, LearningPathDto>();

        var command = new CommandDefinition(sql, cancellationToken: cancellationToken);

        await connection.QueryAsync<LearningPathDto, CourseInPathDto, LearningPathDto>(
        command,
        (pathDto, courseDto) =>
        {
            if (!learningPathDictionary.TryGetValue(pathDto.PathId, out var currentPath))
            {
                currentPath = pathDto with { Courses = new List<CourseInPathDto>() };
                learningPathDictionary.Add(currentPath.PathId, currentPath);
            }

            if (courseDto != null && courseDto.CourseId != 0)
            {
                ((List<CourseInPathDto>)currentPath.Courses).Add(courseDto);
            }

            return currentPath;
        },
        splitOn: "CourseId");

        var learningPaths = learningPathDictionary.Values
        .Select(dto => LearningPathMapper.ToDomain(dto))
        .ToList();

        return learningPaths!;
    }
    public async Task AddCourseToLearningPathAsync(
        int pathId, 
        int courseId, 
        int orderInPath, 
        CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"
            INSERT INTO CourseInPath (LearningPathId, CourseId, OrderInPath)
            VALUES (@LearningPathId, @CourseId, @OrderInPath);";

        var command = new CommandDefinition(
            sql,
            new 
            {
                LearningPathId = pathId, 
                CourseId = courseId, 
                OrderInPath = orderInPath 
            },
            cancellationToken: cancellationToken);

        await connection.ExecuteAsync(command);
    }
    public async Task RemoveCourseFromLearningPathAsync(
        int pathId,
        int courseId,
        CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            DELETE FROM CourseInPath
            WHERE LearningPathId = @LearningPathId AND CourseId = @CourseId;";

        var command = new CommandDefinition(
            sql,
            new { LearningPathId = pathId, CourseId = courseId },
            cancellationToken: cancellationToken);

        await connection.ExecuteAsync(command);
    }
    public void Update(LearningPath learningPath)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            UPDATE LearningPath
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
            Outcomes = learningPath.Outcomes?.Value,
            learningPath.Audience,
            Status = learningPath.Status.Name,
            learningPath.UpdatedAt
        });

        connection.Execute(command);
    }
}
