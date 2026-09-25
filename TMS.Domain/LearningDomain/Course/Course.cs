using TMS.Domain.LearningDomain.Course.Events;
using TMS.Domain.Shared;
using TMS.Domain.Shared.Results;

namespace TMS.Domain.LearningDomain.Course;

/// <summary>
/// The Course is the basic independant unit and is the simplest thing that can be taught in the system.
/// </summary>
public class Course : AggregateRoot<int>
{
    public int SellableItemId { get; private set; }
    public string CourseName { get; private set; } = null!;
    public string? Description { get; private set; }
    public CourseDuration? Duration { get; private set; }
    public CourseStatus Status { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; } = DateTime.UtcNow;

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
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public ResultBase MarkAsPublished()
    {
        if (Status == CourseStatus.Published)
        {
            return CourseError.AlreadyPublished;
        }

        // I change my dessesion here, I want to allow publishing a course even if it is archived,
        // so I will remove the check for archived status.
        this.Status = CourseStatus.Published;
        this.UpdatedAt = DateTime.UtcNow;
        CoursePublishedDomainEvent coursePublishedDomainEvent = new(this.Id);
        AddDomainEvent(coursePublishedDomainEvent);
        return ResultBase.Success();
    }
    public ResultBase MarkAsArchived()
    {
        if (Status == CourseStatus.Archived)
        {
            return CourseError.AlreadyArchived;
        }

        if(Status == CourseStatus.Draft)
        {
            return CourseError.CannotArchiveDraft;
        }
        
        this.Status = CourseStatus.Archived;
        this.UpdatedAt = DateTime.UtcNow;
        CourseArchivedDomainEvent courseArchivedDomainEvent = new(this.Id);
        AddDomainEvent(courseArchivedDomainEvent);
        return ResultBase.Success();
    }
    public ResultBase ChangeName(string courseName)
    {
        if (Status == CourseStatus.Archived)
            return CourseError.InvalidStatus;

        if(string.IsNullOrWhiteSpace(courseName))
            return CourseError.InvalidTitle;

        this.CourseName = courseName;
        this.UpdatedAt = DateTime.UtcNow;
        CourseUpdatedDomainEvent courseUpdatedDomainEvent = new(this.Id);
        AddDomainEvent(courseUpdatedDomainEvent);
        return ResultBase.Success();
    }
    public ResultBase ChangeDescription(string? description)
    {
        if (Status == CourseStatus.Archived)
            return CourseError.InvalidStatus;

        this.Description = description;
        this.UpdatedAt = DateTime.UtcNow;
        CourseUpdatedDomainEvent courseUpdatedDomainEvent = new(this.Id);
        AddDomainEvent(courseUpdatedDomainEvent);
        return ResultBase.Success();
    }
    public ResultBase ChangeDuration(CourseDuration duration)
    {
        if (Status == CourseStatus.Archived)
            return CourseError.InvalidStatus;

        this.Duration = duration;
        this.UpdatedAt = DateTime.UtcNow;
        CourseUpdatedDomainEvent courseUpdatedDomainEvent = new(this.Id);
        AddDomainEvent(courseUpdatedDomainEvent);
        return ResultBase.Success();
    }
}
