using OnlineCoursesApp.Monolith.Application.Models;
using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Application.Common.Interfaces;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Course?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken);

    Task<PagedResult<Course>> GetFilteredAsync(CourseFilter filter, CancellationToken cancellationToken);

    Task AddAsync(Course course, CancellationToken cancellationToken);
    void Update(Course course);

    void Remove(Course course);
}