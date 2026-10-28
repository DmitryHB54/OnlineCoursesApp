using OnlineCoursesApp.Monolith.Application.Models;
using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Application.Common.Interfaces;

public interface ICourseFilteredReadRepository
{
    Task<PagedResult<Course>> GetFilteredAsync(CourseFilter filter, CancellationToken cancellationToken);
}