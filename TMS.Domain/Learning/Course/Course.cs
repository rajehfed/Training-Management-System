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

    private Course() : base(default) { }
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
        Status = CourseStatus.Draft;
    }

    public bool MarkAsPublished(out string message)
    {
        if (Status == CourseStatus.Published)
        {
            message = "The course is already published.";
            return false;
        }

        // I change my dessesion here, I want to allow publishing a course even if it is archived,
        // so I will remove the check for archived status.
        message = "The course has been marked as published.";
        this.Status = CourseStatus.Published;
        return true;
    }
    public bool MarkAsArchived(out string message)
    {
        if (Status == CourseStatus.Archived)
        {
            message = "The course is already archived.";
            return false;
        }

        if(Status == CourseStatus.Draft)
        {
            message = "The course cannot be archived while in draft status.";
            return false;
        }
        
        message = "The course has been marked as archived.";
        this.Status = CourseStatus.Archived;
        return true;
    }
}
