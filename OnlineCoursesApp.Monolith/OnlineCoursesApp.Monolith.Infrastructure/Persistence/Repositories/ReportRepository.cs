using Microsoft.EntityFrameworkCore;
using OnlineCoursesApp.Monolith.Application.Common.Features.Reports.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Repositories;
public class ReportRepository : IReportRepository
{
    private readonly AppDbContext _dbContext;

    public ReportRepository(
        AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<CourseReportRowDto>> GetCourseStatsAsync(
        DateTimeOffset from, DateTimeOffset to, Guid? authorId, CancellationToken cancellationToken)
    {
        var query = _dbContext.Cources
            .AsNoTracking()
            .Where(c => c.CreatedAt >= from && c.CreatedAt <= to);

        if (authorId.HasValue)
        {
            query = query.Where(c => c.AuthorId == authorId.Value);
        }

        var raw = await query
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new
            {
                c.Title,
                AuthorEmail = c.Author.Email,
                c.Category,
                c.Price,
                MaterialsCount = 
                    c.Materials.Count(m => m.CreatedAt >= from && m.CreatedAt <= to),
                EnrollmentsCount = 
                    c.Enrollments.Count(e => e.EnrolledAt >= from && e.EnrolledAt <= to)
            }).ToListAsync(cancellationToken);

        return raw.Select(r => new CourseReportRowDto(
            r.Title,
            r.AuthorEmail,
            r.Category.ToString(),
            r.Price,
            r.MaterialsCount,
            r.EnrollmentsCount)).ToList();
    }
}