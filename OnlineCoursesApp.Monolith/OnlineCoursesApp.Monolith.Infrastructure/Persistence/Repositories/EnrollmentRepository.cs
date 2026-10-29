using Microsoft.EntityFrameworkCore;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Domain.Repositories;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly AppDbContext _dbContext;

    public EnrollmentRepository(
        AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> ExistsAsync(Guid userId, Guid courseId, CancellationToken cancellationToken)
    { 
        return _dbContext.Enrollments.AnyAsync(e => e.UserId == userId && e.CourseId == courseId, cancellationToken);
    }

    public Task<Enrollment?> GetAsync(Guid userId, Guid courseId, CancellationToken cancellationToken)
    { 
        return _dbContext.Enrollments
            .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId, cancellationToken);
    }

    public Task<List<Course>> GetUserCoursesAsync(Guid userId, CancellationToken cancellationToken)
    {
        return _dbContext.Enrollments
            .AsNoTracking()
            .Where(e => e.UserId == userId)
            .Select(e => e.Course)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Enrollment enrollment, CancellationToken cancellationToken)
    {
        await _dbContext.Enrollments.AddAsync(enrollment, cancellationToken);
    }

    public void Remove(Enrollment enrollment)
    {
        _dbContext.Enrollments.Remove(enrollment);
    }
}