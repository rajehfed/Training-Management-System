namespace TMS.Application.Shared.Models;

public record PaginatedList<T>(IReadOnlyCollection<T> Items, int TotalCount, int PageNumber, int PageSize);