using TMS.Application.Shared.Models;
using TMS.Application.Shared.Models.DTOs;
using TMS.Domain.LearningDomain.Course;

namespace TMS.Application.Shared.Interfaces;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Course?> GetByTitleAsync(string title, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Course?>> GetByIdsAsync(
        List<int> Ids,
        CancellationToken cancellationToken = default);
    Task AddAsync(Course course, CancellationToken cancellationToken = default);
    void Update(Course course);
    Task<bool> ExistsAsync(int courseId, CancellationToken cancellationToken);
    Task<PaginatedList<CourseDto>> GetCoursesAsync(int pageNumber = 0, int pageSize = 10, string? searchTerm = null);
}
