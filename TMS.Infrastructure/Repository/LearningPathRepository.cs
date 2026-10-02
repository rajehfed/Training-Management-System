using TMS.Application.Shared.Interfaces;
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

    public Task AddAsync(LearningPath learningPath, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CourseExistsInPath(int pathId, int courseId)
    {
        throw new NotImplementedException();
    }

    public void Delete(LearningPath learningPath)
    {
        throw new NotImplementedException();
    }

    public Task<LearningPath?> GetByIdAsync(int pathId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
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
