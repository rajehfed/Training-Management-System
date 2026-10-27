using MediatR;
using TMS.Application.Shared.Interfaces;
using TMS.Application.Shared.Models.DTOs;

namespace TMS.Application.Learning.LearningPathApp.Queries.GetLearningPathByName;

public class GetLearningPathByNameQueryHandler : IRequestHandler<GetLearningPathByNameQuery, LearningPathDto?>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly ICourseRepository _courseRepository;
    public GetLearningPathByNameQueryHandler(
        ILearningPathRepository learningPathRepository, ICourseRepository courseRepository)
    {
        _learningPathRepository = learningPathRepository;
        _courseRepository = courseRepository;
    }
    public async Task<LearningPathDto?> Handle(GetLearningPathByNameQuery request, CancellationToken cancellationToken)
    {
        var path = await _learningPathRepository.GetByNameAsync(request.PathName, cancellationToken);

        if (path is null)
            return null;

        var courseIds = path.Courses.Select(c => c.CourseId).ToList();

        var courses = await _courseRepository.GetByIdsAsync(courseIds, cancellationToken);

        var courseDtos = path.Courses
            .Select(cip =>
            {
                var course = courses.FirstOrDefault(c => c.Id == cip.CourseId);
                return new CourseInPathDto
                {
                    CourseId = cip.CourseId,
                    PathId = cip.LearningPathId,
                    CourseName = course?.CourseName ?? "Unknown Course",
                    OrderInPath = cip.OrderInPath,
                    Description = course?.Description
                };
            })
            .OrderBy(c => c.OrderInPath)
            .ToList();

        return new LearningPathDto
        {
            PathId = path.Id,
            SellableItemId = path.SellableItemId,
            PathName = path.PathName,
            Outcomes = path.Outcomes.ToString(),
            Audience = path.Audience,
            Status = path.Status.Name,
            CreatedAt = path.CreatedAt,
            UpdatedAt = path.UpdatedAt,
            Courses = courseDtos
        };
    }
}
