using TMS.Application.Shared.Models.DTOs;
using TMS.Domain.Training.TrainingBatch;
using TMS.Domain.TrainingDomain.Session;

namespace TMS.Infrastructure.Mappers;

public static class TrainingBatchMapper
{
    public static TrainingBatch ToDomain(this TrainingBatchDto dto)
    {
        return new TrainingBatch(
            dto.BatchId,
            dto.CourseID,
            dto.MentorID,
            dto.StartDate,
            dto.EndDate,
            TrainingBatchStatus.FromName(dto.Status),
            dto.CreatedAt,
            dto.UpdatedAt,
            [.. dto.Sessions.Select(s => new Session(
                s.SessionId,
                s.BatchId,
                s.StartTime,
                s.EndTime,
                s.RoomId,
                SessionStatus.FromName(s.Status),
                s.CreatedAt,
                s.UpdatedAt))]);
    }
}
