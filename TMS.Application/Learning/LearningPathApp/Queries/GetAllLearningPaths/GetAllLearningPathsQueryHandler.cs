using MediatR;
using TMS.Application.Shared.Interfaces;
using TMS.Application.Shared.Models.DTOs;

namespace TMS.Application.Learning.LearningPathApp.Queries.GetAllLearningPaths;

public class GetAllLearningPathsQueryHandler : IRequestHandler<GetAllLearningPathsQuery, List<LearningPathDto>?>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly ICourseRepository _courseRepository;

    public GetAllLearningPathsQueryHandler(ILearningPathRepository learningPathRepository, ICourseRepository courseRepository)
    {
        _learningPathRepository = learningPathRepository;
        _courseRepository = courseRepository;
    }
    public async Task<List<LearningPathDto>?> Handle(GetAllLearningPathsQuery request, CancellationToken cancellationToken)
    {
        var path = await _learningPathRepository.GetAllLearningPathsAsync(cancellationToken);

        if (path.Count == 0)
            return null;

        var coursesIds = path.Where(lp => lp.Courses != null)
            .SelectMany(lp => lp.Courses.Select(c => c.CourseId))
            .Distinct()
            .ToList();

        var courses = await _courseRepository.GetByIdsAsync(coursesIds, cancellationToken);

        var result = path.Select(lp => new LearningPathDto
        {
            PathId = lp.Id,
            SellableItemId = lp.SellableItemId,
            PathName = lp.PathName,
            Outcomes = lp.Outcomes.ToString(),
            Audience = lp.Audience,
            Status = lp.Status.Name,
            CreatedAt = lp.CreatedAt,
            UpdatedAt = lp.UpdatedAt,

            Courses = lp.Courses?
                .OrderBy(cip => cip.OrderInPath)
                .Select(cip =>
                {
                    var courseInfo = courses.FirstOrDefault(c => c.Id == cip.CourseId);
                    return new CourseInPathDto
                    {
                        CourseId = cip.CourseId,
                        PathId = cip.LearningPathId,
                        CourseName = courseInfo?.CourseName ?? "Unknown Course",
                        OrderInPath = cip.OrderInPath,
                        Description = courseInfo?.Description
                    };
                }).ToList() ?? []
        }).ToList();

        return result;
    }
}
