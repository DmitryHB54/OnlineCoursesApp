using OnlineCoursesApp.Monolith.Application.Models;
using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Application.Common.Interfaces;
public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<PagedResult<Course>> GetFilteredAsync(CourseFilter filter, CancellationToken ct);
    Task AddAsync(Course course, CancellationToken ct);
    void Update(Course course);
    void Remove(Course course);
}