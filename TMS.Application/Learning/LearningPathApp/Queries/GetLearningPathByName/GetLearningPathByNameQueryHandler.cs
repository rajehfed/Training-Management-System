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
                return new CourseInPathDto(
                    cip.CourseId,
                    course?.CourseName ?? "Unknown Course",
                    cip.OrderInPath,
                    course?.Description
                );
            })
            .OrderBy(c => c.SequenceOrder)
            .ToList();

        return new LearningPathDto(
            path.Id,
            path.SellableItemId,
            path.PathName,
            path.Outcomes.ToString(),
            path.Audience,
            path.Status.Name,
            courseDtos,
            path.CreatedAt,
            path.UpdatedAt);
    }
}
