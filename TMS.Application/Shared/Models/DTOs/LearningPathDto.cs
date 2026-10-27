namespace TMS.Application.Shared.Models.DTOs;

public record CourseInPathDto
{
    public CourseInPathDto() { }

    public int CourseId { get; init; }
    public int PathId { get; init; }
    public string CourseName { get; init; } = string.Empty;
    public int OrderInPath { get; init; }
    public string? Description { get; init; }
}

public record LearningPathDto
{
    public LearningPathDto() { }

    public int PathId { get; init; }
    public int SellableItemId { get; init; }
    public string PathName { get; init; } = string.Empty;
    public string Outcomes { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }

    public IReadOnlyCollection<CourseInPathDto> Courses { get; init; } = [];
}