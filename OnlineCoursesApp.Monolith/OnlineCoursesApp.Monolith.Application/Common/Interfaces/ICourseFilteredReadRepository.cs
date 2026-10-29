using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;
using OnlineCoursesApp.Monolith.Application.Models;

namespace OnlineCoursesApp.Monolith.Application.Common.Interfaces;

public interface ICourseFilteredReadRepository
{
    Task<PagedResult<CourseDto>> GetFilteredAsync(CourseFilter filter, CancellationToken cancellationToken);
}