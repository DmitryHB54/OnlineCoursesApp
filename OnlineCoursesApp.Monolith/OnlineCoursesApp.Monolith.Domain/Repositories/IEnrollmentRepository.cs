using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Domain.Repositories;

public interface IEnrollmentRepository : IRepository<Enrollment>
{
    Task<bool> ExistsAsync(Guid userId, Guid courseId, CancellationToken cancellationToken);

    Task<Enrollment?> GetAsync(Guid userId, Guid courseId, CancellationToken cancellationToken);

    Task<List<Course>> GetUserCoursesAsync(Guid userId, CancellationToken cancellationToken);
}