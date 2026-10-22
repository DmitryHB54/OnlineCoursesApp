namespace OnlineCoursesApp.Monolith.Application.Models;

public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalCount)
{
    public int TotaPages
        => (int)Math.Ceiling(TotalCount / (double)PageSize);
}