using Dapper;
using TMS.Application.Shared.Interfaces;
using TMS.Domain.LearningDomain.Course;
using TMS.Infrastructure.Dapper;

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

        const string sqlCourseQuery = @"
            INSERT INTO Course (
                SellableItemId, 
                CourseName, 
                Description, 
                Duration,
                CreatedAt,
                UpdatedAt)
            VALUES (
                @SellableItemId, 
                @CourseName, 
                @Description, 
                @Duration,
                @CreatedAt,
                @UpdatedAt)
        ";

        await connection.ExecuteAsync(sqlCourseQuery, new
        {
            course.SellableItemId,
            course.CourseName,
            course.Description,
            course.Duration,
            course.CreatedAt,
            course.UpdatedAt
        });
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

        return await connection.QueryFirstOrDefaultAsync<Course>(
            new CommandDefinition(
                sql,
                new { CourseId = id },
                cancellationToken: cancellationToken)
            );
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

        var courses = await connection.QueryAsync<Course>(
            new CommandDefinition(
                sql,
                new { CourseIds = Ids.ToArray() },
                cancellationToken: cancellationToken));

        return courses.ToList();
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

        return await connection.QueryFirstOrDefaultAsync<Course>(
            new CommandDefinition(
                sql,
                new { CourseName = title },
                cancellationToken: cancellationToken)
            );
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
            course.Duration,
            course.Status,
            course.CreatedAt,
            course.UpdatedAt,
            CourseId = course.Id
        });
    }
}
