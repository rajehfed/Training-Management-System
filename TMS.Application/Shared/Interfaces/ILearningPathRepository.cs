using TMS.Application.Shared.Models.DTOs;
using TMS.Domain.LearningDomain.LearningPath;

namespace TMS.Application.Shared.Interfaces;

public interface ILearningPathRepository
{
    Task<LearningPath?> GetByIdAsync(int pathId, CancellationToken cancellationToken = default);
    Task<LearningPath?> GetByNameAsync(string pathName, CancellationToken cancellationToken = default);
    Task AddAsync(LearningPath learningPath, CancellationToken cancellationToken = default);
    Task<bool> CourseExistsInPath(int pathId, int courseId, CancellationToken cancellationToken = default);
    Task<List<LearningPath>?> GetAllLearningPathsAsync(CancellationToken cancellationToken);
    void Update(LearningPath learningPath);
    void Delete(LearningPath learningPath);
}
