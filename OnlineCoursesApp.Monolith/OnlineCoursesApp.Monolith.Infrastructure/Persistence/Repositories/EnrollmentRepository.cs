using Microsoft.EntityFrameworkCore;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Repositories;
public class EnrollmentRepository(
    AppDbContext dbContext) : IEnrollmentRepository
{
    public Task<bool> ExistsAsync(Guid userId, Guid courseId, CancellationToken ct)
        => dbContext.Enrollments.AnyAsync(e => e.UserId == userId && e.CourseId == courseId, ct);
}