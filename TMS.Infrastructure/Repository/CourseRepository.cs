using Dapper;
using MediatR;
using TMS.Application.Shared.Interfaces;
using TMS.Application.Shared.Models;
using TMS.Application.Shared.Models.DTOs;
using TMS.Domain.LearningDomain.Course;
using TMS.Infrastructure.Dapper;
using TMS.Infrastructure.Mappers;

namespace TMS.Infrastructure.Repository;

public class CourseRepository : ICourseRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public CourseRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task AddAsync(Course course, CancellationToken cancellationToken = default)
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
                    new { ItemType = "Course", course.CreatedAt },
                    transaction: transaction,
                    cancellationToken: cancellationToken)
                );

            const string sqlCourseQuery = @"
                INSERT INTO Course (
                    SellableItemId, 
                    CourseName, 
                    Description, 
                    Duration,
                    Status,
                    CreatedAt,
                    UpdatedAt)
                VALUES (
                    @SellableItemId, 
                    @CourseName, 
                    @Description, 
                    @Duration,
                    @Status,
                    @CreatedAt,
                    @UpdatedAt)
            ";

            var courseCommand = new CommandDefinition(
                sqlCourseQuery,
                new
                {
                    SellableItemId = sellableItemId,
                    course.CourseName,
                    course.Description,
                    Duration = course.Duration.Minutes,
                    Status = course.Status.ToString(),
                    course.CreatedAt,
                    course.UpdatedAt
                },
                transaction: transaction,
                cancellationToken: cancellationToken);

            await connection.ExecuteAsync(courseCommand);
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int courseId, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sqlCourseQuery = @"SELECT EXISTS (
            SELECT 1 
            FROM Course 
            WHERE CourseId = @CourseId
        );";

        var command = new CommandDefinition(
            sqlCourseQuery,
            new { CourseId = courseId },
            cancellationToken: cancellationToken);

        return await connection.ExecuteScalarAsync<bool>(command);
    }

    public async Task<Course?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            SELECT 
                CourseId AS Id, 
                SellableItemId, 
                CourseName, 
                Description, 
                Duration, 
                Status, 
                CreatedAt, 
                UpdatedAt 
            FROM Course 
            WHERE CourseId = @CourseId;
        ";

        var command = new CommandDefinition(
                sql,
                new { CourseId = id },
                cancellationToken: cancellationToken);

        var dto = await connection.QueryFirstOrDefaultAsync<CourseDto>(command);
        if (dto is null) return null;

        return CourseMapper.ToDomain(dto);
    }

    public async Task<IReadOnlyCollection<Course?>> GetByIdsAsync(List<int> Ids, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @" 
            SELECT 
                CourseId AS Id, 
                SellableItemId, 
                CourseName, 
                Description, 
                Duration, 
                Status, 
                CreatedAt, 
                UpdatedAt 
            FROM Course 
            WHERE CourseId = ANY(@CourseIds);";

        var command = new CommandDefinition(
            sql,
            new { CourseIds = Ids.ToArray() },
            cancellationToken: cancellationToken);

        var dtos = await connection.QueryAsync<CourseDto>(command);
        if(dtos is null) return [];

        return dtos.Select(dto => CourseMapper.ToDomain(dto)).ToList();
    }

    public async Task<Course?> GetByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            SELECT 
                CourseId AS Id, 
                SellableItemId, 
                CourseName, 
                Description, 
                Duration, 
                Status, 
                CreatedAt, 
                UpdatedAt 
            FROM Course 
            WHERE CourseName = @CourseName;
        ";

        var command = new CommandDefinition(
                sql,
                new { CourseName = title },
                cancellationToken: cancellationToken);

        var dto = await connection.QueryFirstOrDefaultAsync<CourseDto>(command);
        if (dto is null) return null;

        return CourseMapper.ToDomain(dto);
    }

    public async Task<PaginatedList<CourseDto>> GetCoursesAsync(int pageNumber = 0, int pageSize = 10, string? searchTerm = null)
    {
        using var connection = _connectionFactory.CreateConnection();

        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        var offset = (pageNumber - 1) * pageSize;

        const string sql = @"
            SELECT 
                CourseId AS Id,
                SellableItemId,
                CourseName,
                Description,
                Duration,
                Status,
                CreatedAt,
                UpdatedAt,
                COUNT(*) OVER() AS TotalCount
            FROM Course 
            WHERE (@SearchTerm IS NULL OR CourseName ILIKE '%' || @SearchTerm || '%')
            ORDER BY CreatedAt DESC
            LIMIT @PageSize OFFSET @Offset;";

        var parameters = new
        {
            SearchTerm = searchTerm,
            PageSize = pageSize,
            Offset = offset
        };

        var rawResults = await connection.QueryAsync<CourseQueryResult>(sql, parameters);
        var resultsList = rawResults.ToList();

        if(resultsList.Count == 0)
            return new PaginatedList<CourseDto>([], 0, pageNumber, pageSize);

        var totalCount = resultsList.First().TotalCount;

        var dtos = resultsList.Select(c => new CourseDto
        (
            Id: c.Id,
            SellableItemId: c.SellableItemId,
            CourseName: c.CourseName,
            Description: c.Description,
            Duration: c.Duration,
            Status: c.Status,
            CreatedAt: c.CreatedAt,
            UpdatedAt: c.UpdatedAt
        )).ToList();

        return new PaginatedList<CourseDto>(dtos, totalCount, pageNumber, pageSize);
    }

    public void Update(Course course)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sqlCourseQuery = @"
            UPDATE Course
            SET 
                SellableItemId = @SellableItemId,
                CourseName = @CourseName,
                Description = @Description,
                Duration = @Duration,
                Status = @Status,
                CreatedAt = @CreatedAt,
                UpdatedAt = @UpdatedAt
            WHERE CourseId = @CourseId";

        connection.Execute(sqlCourseQuery, new
        {
            course.SellableItemId,
            course.CourseName,
            course.Description,
            Duration = course.Duration.Minutes,
            Status = course.Status.ToString(),
            course.CreatedAt,
            course.UpdatedAt,
            CourseId = course.Id
        });
    }
}
