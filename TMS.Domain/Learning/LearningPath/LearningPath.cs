using TMS.Domain.Learning.Course;
using TMS.Domain.Learning.Course.Events;
using TMS.Domain.Learning.LearningPath.Events;
using TMS.Domain.Shared;
using TMS.Domain.Shared.Results;

namespace TMS.Domain.Learning.LearningPath;

public class LearningPath : AggregateRoot<int>
{
    public int SellableItemId { get; private set; }
    public string PathName { get; private set; } = null!;
    public Outcomes? Outcomes { get; private set; }
    public string? Audience { get; private set; }
    public PathStatus Status { get; private set; } = PathStatus.Draft;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    protected LearningPath() : base(default) { }
    public LearningPath(
        int id,
        int sellableItemId,
        string pathName,
        Outcomes? outcomes,
        string? audience) : base(id)
    {
        if (string.IsNullOrWhiteSpace(pathName))
            throw new ArgumentException("The path name cannot be empty.", nameof(pathName));

        SellableItemId = sellableItemId;
        PathName = pathName;
        Outcomes = outcomes;
        Audience = audience;
        Status = PathStatus.Draft;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public ResultBase MarkAsPublished()
    {
        if (Status == PathStatus.Published)
        {
            return LearningPathError.AlreadyPublished;
        }
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
        LearningPathArchivedDomainEvent LearningPathUpdatedDomainEvent = new(this.Id);
        AddDomainEvent(LearningPathUpdatedDomainEvent);
        return ResultBase.Success();
    }
    public ResultBase ChangeOutcomes(Outcomes? outcomes)
    {
        if (Status == PathStatus.Archived)
            return LearningPathError.InvalidStatus;

        this.Outcomes = outcomes;
        this.UpdatedAt = DateTime.UtcNow;
        LearningPathArchivedDomainEvent LearningPathUpdatedDomainEvent = new(this.Id);
        AddDomainEvent(LearningPathUpdatedDomainEvent);
        return ResultBase.Success();
    }
    public ResultBase ChangeAudience(string? audience)
    {
        if (Status == PathStatus.Archived)
            return LearningPathError.InvalidStatus;

        this.Audience = audience;
        this.UpdatedAt = DateTime.UtcNow;
        LearningPathArchivedDomainEvent LearningPathUpdatedDomainEvent = new(this.Id);
        AddDomainEvent(LearningPathUpdatedDomainEvent);
        return ResultBase.Success();
    }
}
