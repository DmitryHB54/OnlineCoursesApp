using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Application.Common.Interfaces;
public interface IEnrollmentRepository
{
    Task<bool> ExistsAsync(Guid userId, Guid courseId, CancellationToken ct);
    Task<Enrollment?> GetAsync(Guid userId, Guid courseId, CancellationToken ct);
    Task<List<Course>> GetUserCoursesAsync(Guid userId, CancellationToken ct);
    Task AddAsync(Enrollment enrollment, CancellationToken ct);
    void Remove(Enrollment enrollment);
}