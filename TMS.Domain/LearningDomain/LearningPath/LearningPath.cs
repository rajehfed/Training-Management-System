using TMS.Domain.LearningDomain.Course;
using TMS.Domain.LearningDomain.Course.Events;
using TMS.Domain.LearningDomain.CourseInPath;
using TMS.Domain.LearningDomain.LearningPath.Events;
using TMS.Domain.Shared;
using TMS.Domain.Shared.Results;

namespace TMS.Domain.LearningDomain.LearningPath;

public class LearningPath : AggregateRoot<int>
{
    public int SellableItemId { get; private set; }
    public string PathName { get; private set; } = null!;
    public Outcomes? Outcomes { get; private set; }
    public string? Audience { get; private set; }
    public PathStatus Status { get; private set; } = PathStatus.Draft;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    private List<CourseInPath.CourseInPath> _courses = new();
    public IReadOnlyCollection<CourseInPath.CourseInPath> Courses => _courses.AsReadOnly();
    protected LearningPath() : base(default) { }
    public LearningPath(
        int id,
        int sellableItemId,
        string pathName,
        Outcomes? outcomes,
        string? audience,
        PathStatus status,
        DateTime createdAt,
        DateTime? updatedAt,
        List<CourseInPath.CourseInPath> courses) : base(id)
    {
        if (string.IsNullOrWhiteSpace(pathName))
            throw new ArgumentException("The path name cannot be empty.", nameof(pathName));

        SellableItemId = sellableItemId;
        PathName = pathName;
        Outcomes = outcomes;
        Audience = audience;
        CreatedAt = createdAt;
        Status = status;
        UpdatedAt = updatedAt;
        _courses = courses;
    }

    public static LearningPath Create(string name, Outcomes outcomes, string? audience)
    {
        return new LearningPath(
            0,
            0,
            name,
            outcomes,
            audience,
            PathStatus.Draft,
            DateTime.Now,
            null,
            []);
    }
    public ResultBase MarkAsPublished()
    {
        if (Status == PathStatus.Published)
            return LearningPathError.AlreadyPublished;

        this.Status = PathStatus.Published;
        this.UpdatedAt = DateTime.UtcNow;
        LearningPathPublishedDomainEvent learningPathPublishedDomainEvent = new(this.Id);
        AddDomainEvent(learningPathPublishedDomainEvent);
        return ResultBase.Success();
    }
    public ResultBase MarkAsArchived()
    {
        if (Status == PathStatus.Archived)
            return LearningPathError.AlreadyArchived;
        if (Status == PathStatus.Draft)
            return LearningPathError.CannotArchiveDraft;

        this.Status = PathStatus.Archived;
        this.UpdatedAt = DateTime.UtcNow;
        LearningPathArchivedDomainEvent learningPathArchivedDomainEvent = new(this.Id);
        AddDomainEvent(learningPathArchivedDomainEvent);
        return ResultBase.Success();
    }
    public ResultBase ChangeName(string pathName)
    {
        if (Status == PathStatus.Archived)
            return LearningPathError.InvalidStatus;

        if (string.IsNullOrWhiteSpace(pathName))
            return LearningPathError.InvalidPathName;

        this.PathName = pathName;
        this.UpdatedAt = DateTime.UtcNow;
        LearningPathUpdatedDomainEvent LearningPathUpdatedDomainEvent = new(this.Id);
        AddDomainEvent(LearningPathUpdatedDomainEvent);
        return ResultBase.Success();
    }
    public ResultBase ChangeOutcomes(Outcomes? outcomes)
    {
        if (Status == PathStatus.Archived)
            return LearningPathError.InvalidStatus;

        this.Outcomes = outcomes;
        this.UpdatedAt = DateTime.UtcNow;
        LearningPathUpdatedDomainEvent LearningPathUpdatedDomainEvent = new(this.Id);
        AddDomainEvent(LearningPathUpdatedDomainEvent);
        return ResultBase.Success();
    }
    public ResultBase ChangeAudience(string? audience)
    {
        if (Status == PathStatus.Archived)
            return LearningPathError.InvalidStatus;

        this.Audience = audience;
        this.UpdatedAt = DateTime.UtcNow;
        LearningPathUpdatedDomainEvent LearningPathUpdatedDomainEvent = new(this.Id);
        AddDomainEvent(LearningPathUpdatedDomainEvent);
        return ResultBase.Success();
    }

    public ResultBase AddCourseToPath(int courseId, int orderInThePath)
    {
        if (Status == PathStatus.Archived)
            return LearningPathError.InvalidStatus;

        if (courseId < 0)
            return LearningPathError.InvalidCourseId;

        if (_courses.Any(c => c.CourseId == courseId))
            return LearningPathError.CourseAlreadyInPath;

        if (orderInThePath <= 0)
            return LearningPathError.InvalidOrderInPath;

        _courses.Add(new CourseInPath.CourseInPath(courseId, this.Id, orderInThePath));
        this.UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new AddingCourseToPathDomainEvent(this.Id));

        return ResultBase.Success();
    }
    public ResultBase ChangeCourseOrderInPath(int courseId, int orderInThePath)
    {
        if (courseId < 0)
            return LearningPathError.InvalidCourseId;

        if (orderInThePath <= 0 || orderInThePath > _courses.Count + 1)
            return LearningPathError.InvalidOrderInPath;

        if (Status == PathStatus.Archived)
            return LearningPathError.InvalidStatus;

        if (!_courses.Any(c => c.CourseId == courseId))
            return LearningPathError.CourseNotFoundInThePath;

        var courseInPath = _courses.First(c => c.CourseId == courseId);

        foreach (var course in _courses)
        {
            if (course.OrderInPath >= orderInThePath && course.CourseId != courseId)
            {
                course.UpdateOrderInPath(course.OrderInPath + 1);
            }
        }

        courseInPath.UpdateOrderInPath(orderInThePath);
        AddDomainEvent(new ChangingCourseOrderInPathChangedDomainEvent(this.Id));
        return ResultBase.Success();
    }
    public ResultBase RemoveCourseFromPath(int courseId)
    {
        if (Status == PathStatus.Archived)
            return LearningPathError.InvalidStatus;

        var courseInPath = _courses.FirstOrDefault(c => c.CourseId == courseId);

        if (courseInPath is null)
            return LearningPathError.CourseNotFoundInThePath;


        foreach (var course in _courses)
        {
            if (course.OrderInPath >= courseInPath.OrderInPath && course.CourseId != courseId)
                course.UpdateOrderInPath(course.OrderInPath - 1);
        }

        _courses.Remove(courseInPath);
        this.UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new RemovingCourseFromPathDomainEvent(this.Id));
        return ResultBase.Success();
    }

    public ResultBase UpdateDetails(string pathName, string outcomes, string audience)
    {
        if (Status == PathStatus.Archived)
            return LearningPathError.InvalidStatus;

        if (string.IsNullOrWhiteSpace(pathName) || 
            string.IsNullOrWhiteSpace(outcomes) || 
            string.IsNullOrWhiteSpace(audience))
            return LearningPathError.InvalidInputs;

        this.PathName = pathName;
        this.Outcomes = new Outcomes(outcomes.Split([',']));
        this.Audience = audience;

        this.AddDomainEvent(new LearningPathUpdatedDomainEvent(this.Id));
        return ResultBase.Success();
    }
}
