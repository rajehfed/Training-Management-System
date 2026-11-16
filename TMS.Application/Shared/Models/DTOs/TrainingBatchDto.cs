namespace TMS.Application.Shared.Models.DTOs;

public record SessionDto
{
    public SessionDto() { }

    public int SessionId { get; init; }
    public int BatchId { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public int? RoomId { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

public record TrainingBatchDto
{
    public TrainingBatchDto() { }
    public int BatchId { get; init; }
    public int CourseID { get; init; }
    public int? MentorID { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }

    public IReadOnlyCollection<SessionDto> Sessions { get; init; } = [];
}
