namespace TMS.Application.Shared.Models.DTOs;

public record CourseDto(
    int Id,
    int SellableItemId,
    string CourseName,
    string? Description,
    int? Duration,
    string Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt);