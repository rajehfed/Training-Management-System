using System.Text;
using TMS.Domain.Shared;

namespace TMS.Domain.Learning.Course;

/// <summary>
/// The Course is the basic independant unit and is the simplest thing that can be taught in the system.
/// </summary>
public class Course : Entity<int>
{
    public int SellableItemId { get; private set; }
    public string CourseName { get; private set; } = null!;
    public string? Description { get; private set; }
    public CourseDuration Duration { get; private set; }
    public CourseStatus Status { get; private set; } = CourseStatus.Draft;

    public Course() : base(default) { }
    public Course(
        int id,
        int sellableItemId,
        string courseName,
        string? description,
        CourseDuration duration) : base(id)
    {
        if (string.IsNullOrWhiteSpace(courseName))
            throw new ArgumentException("The course name cannot be empty.", nameof(courseName));

        SellableItemId = sellableItemId;
        CourseName = courseName;
        Description = description;
        Duration = duration;
    }
}
