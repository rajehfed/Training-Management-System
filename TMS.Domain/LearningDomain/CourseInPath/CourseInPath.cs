using TMS.Domain.Learning.LearningPath;
using TMS.Domain.Learning.LearningPath.Events;
using TMS.Domain.Shared;
using TMS.Domain.Shared.Results;

namespace TMS.Domain.LearningDomain.CourseInPath;

public record CourseInPathId(int CourseId, int LearningPathId);

public class CourseInPath : Entity<CourseInPathId>
{
    public int CourseId { get; private set; }
    public int LearningPathId { get; private set; }
    public int OrderInPath { get; private set; }
    public LearningPath LearningPath { get; private set; } = null!;
    public CourseInPath() : base(default!) { }

    public CourseInPath(
        int courseId,
        int learningPathId,
        int orderInPath) : base(new CourseInPathId(courseId, learningPathId))
    {
        CourseId = courseId;
        LearningPathId = learningPathId;
        OrderInPath = orderInPath;
    }

    internal void UpdateOrderInPath(int orderInPath)
    {
        OrderInPath = orderInPath;
    }
}
