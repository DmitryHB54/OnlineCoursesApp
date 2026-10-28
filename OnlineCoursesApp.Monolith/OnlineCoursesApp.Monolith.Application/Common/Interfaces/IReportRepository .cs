using OnlineCoursesApp.Monolith.Application.Common.Features.Reports.Dtos;

namespace OnlineCoursesApp.Monolith.Application.Common.Interfaces;

public interface IReportRepository
{
    Task<List<CourseReportRowDto>> GetCourseStatsAsync(
        DateTimeOffset from,
        DateTimeOffset to,
        Guid? authorId,
        CancellationToken ct);
}