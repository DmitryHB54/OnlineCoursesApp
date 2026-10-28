using Microsoft.EntityFrameworkCore;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Application.Models;
using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _dbContext;

    public CourseRepository(
        AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Cources
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task<Course?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Cources
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<PagedResult<Course>> GetFilteredAsync(CourseFilter filter, CancellationToken cancellationToken)
    {
        var query = _dbContext.Cources.AsNoTracking().AsQueryable();

        if (filter.Category.HasValue)
        {
            query = query.Where(c => c.Category == filter.Category.Value);
        }

        if (filter.AuthorId.HasValue)
        {
            query = query.Where(c => c.AuthorId == filter.AuthorId.Value);
        }

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

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Course>(items, filter.Page, filter.PageSize, total);
    }

    public async Task AddAsync(Course course, CancellationToken cancellationToken)
    { 
        await _dbContext.Cources.AddAsync(course, cancellationToken);         
    }

    public void Update(Course course)
    { 
        _dbContext.Cources.Update(course);
    }

    public void Remove(Course course)
    { 
        _dbContext.Cources.Remove(course);
    } 
}