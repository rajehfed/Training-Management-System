using System.Runtime.CompilerServices;
using TMS.Domain.Shared;

namespace TMS.Domain.Learning.Course;

public abstract class CourseStatus : Enumeration<CourseStatus>
{
    public static readonly CourseStatus Draft = new DraftStatus();
    public static readonly CourseStatus Published = new PublishedStatus();
    public static readonly CourseStatus Archived = new ArchivedStatus();

    private CourseStatus(int id, string name) : base(id, name) { }

    private sealed class DraftStatus() : CourseStatus(1, "Draft") { }
    private sealed class PublishedStatus() : CourseStatus(2, "Published") { }
    private sealed class ArchivedStatus() : CourseStatus(3, "Archived") { }

}
