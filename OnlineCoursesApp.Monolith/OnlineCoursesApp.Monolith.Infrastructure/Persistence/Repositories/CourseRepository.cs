using Microsoft.EntityFrameworkCore;
using OnlineCoursesApp.Monolith.Application.Models;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Domain.Repositories;

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

    public Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Cources
            .AnyAsync(c => c.Id == id, cancellationToken);
    }

    public Task<Course?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Cources
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
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