using TMS.Domain.Learning.Course;
using TMS.Domain.Shared;

namespace TMS.Domain.Learning.LearningPath;

public abstract class PathStatus : Enumeration<PathStatus>
{
    public static readonly PathStatus Draft = new DraftStatus();
    public static readonly PathStatus Published = new PublishedStatus();
    public static readonly PathStatus Archived = new ArchivedStatus();
    protected PathStatus(int id, string name) : base(id, name) { }
    private sealed class DraftStatus() : PathStatus(1, "Draft") { }
    private sealed class PublishedStatus() : PathStatus(2, "Published") { }
    private sealed class ArchivedStatus() : PathStatus(3, "Archived") { }
}
