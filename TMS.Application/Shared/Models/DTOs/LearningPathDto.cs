namespace TMS.Application.Shared.Models.DTOs;

public record CourseInPathDto(
    int CourseId,
    string CourseTitle,
    int SequenceOrder,
    string? Description
);

public record LearningPathDto(
    int PathId,
    int SellableItemId,
    string PathName,
    string Outcomes,
    string Audience,
    string Status,
    IReadOnlyCollection<CourseInPathDto> Courses,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
