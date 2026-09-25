using TMS.Domain.LearningDomain.Course;

namespace TMS.Application.Shared.Interfaces;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Course course, CancellationToken cancellationToken = default);
    void Update(Course course);
}
