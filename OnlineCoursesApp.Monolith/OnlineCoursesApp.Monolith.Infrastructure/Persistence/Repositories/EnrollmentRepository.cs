using Microsoft.EntityFrameworkCore;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Repositories;
public class EnrollmentRepository(
    AppDbContext dbContext) : IEnrollmentRepository
{
    public Task<bool> ExistsAsync(Guid userId, Guid courseId, CancellationToken ct)
        => dbContext.Enrollments.AnyAsync(e => e.UserId == userId && e.CourseId == courseId, ct);
    public Task<Enrollment?> GetAsync(Guid userId, Guid courseId, CancellationToken ct)
        => dbContext.Enrollments
            .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId, ct);
    public Task<List<Course>> GetUserCoursesAsync(Guid userId, CancellationToken ct)
        => dbContext.Enrollments
            .AsNoTracking()
            .Where(e => e.UserId == userId)
            .Select(e => e.Course)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

    public async Task AddAsync(Enrollment enrollment, CancellationToken ct)
        => await dbContext.Enrollments.AddAsync(enrollment, ct);

    public void Remove(Enrollment enrollment)
        => dbContext.Enrollments.Remove(enrollment);
}