using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Application.Common.Interfaces;

public interface IEnrollmentRepository
{
    Task<bool> ExistsAsync(Guid userId, Guid courseId, CancellationToken cancellationToken);

    Task<Enrollment?> GetAsync(Guid userId, Guid courseId, CancellationToken cancellationToken);

    Task<List<Course>> GetUserCoursesAsync(Guid userId, CancellationToken cancellationToken);

    Task AddAsync(Enrollment enrollment, CancellationToken cancellationToken);

    void Remove(Enrollment enrollment);
}