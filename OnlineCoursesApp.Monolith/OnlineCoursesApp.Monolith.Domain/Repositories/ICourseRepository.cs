using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Domain.Repositories;

public interface ICourseRepository : IRepository<Course>
{
    Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Course?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken);

    void Update(Course course);
}