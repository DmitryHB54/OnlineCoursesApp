namespace OnlineCoursesApp.Monolith.Application.Models;

public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalCount)
{
    public int TotaPages
    {
        get
        {
            return (int)Math.Ceiling(TotalCount / (double)PageSize);
        }
    }
}