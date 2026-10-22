using Microsoft.EntityFrameworkCore;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Application.Models;
using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Repositories;

public class CourseRepository(
    AppDbContext dbContext) : ICourseRepository
{
    public Task<Course?> GetByIdAsync(Guid id, CancellationToken ct)
        => dbContext.Cources
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public Task<Course?> GetByIdForUpdateAsync(Guid id, CancellationToken ct)
        => dbContext.Cources.FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<PagedResult<Course>> GetFilteredAsync(CourseFilter filter, CancellationToken ct)
    {
        var query = dbContext.Cources.AsNoTracking().AsQueryable();

        if (filter.Category.HasValue)
            query = query.Where(c => c.Category == filter.Category.Value);

        if (filter.AuthorId.HasValue)
            query = query.Where(c => c.AuthorId == filter.AuthorId.Value);

        if(!string.IsNullOrWhiteSpace(filter.Search))
        {
            var pattern = $"%{filter.Search.Trim()}%";
            query = query.Where(c =>
                EF.Functions.ILike(c.Title, pattern) ||
                EF.Functions.ILike(c.Description ?? string.Empty, pattern)
            );
        }

        query = filter.SortBy?.ToLowerInvariant() switch
        {
            "title" => query.OrderBy(c => c.Title),
            "price" => query.OrderBy(c => c.Price),
            "createdat" => query.OrderByDescending(c => c.CreatedAt),
            _ => query.OrderByDescending(c => c.CreatedAt)
        };

        var total = await query.CountAsync(ct);

        var items = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(ct);

        return new PagedResult<Course>(items, filter.Page, filter.PageSize, total);
    }

    public async Task AddAsync(Course course, CancellationToken ct)
        => await dbContext.Cources.AddAsync(course, ct);
    public void Update(Course course) => dbContext.Cources.Update(course);
    public void Remove(Course course) => dbContext.Cources.Remove(course);

}