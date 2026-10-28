using Microsoft.Extensions.Logging;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Application.Models;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Infrastructure.Persistence.Repositories;
using System.Reflection;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Decorators;

public class LoggingCourseFilteredReadRepository : ICourseFilteredReadRepository
{
    private readonly ICourseFilteredReadRepository _inner;
    private readonly ILogger<CourseFilteredReadRepository> _logger;

    public LoggingCourseFilteredReadRepository(
        ICourseFilteredReadRepository inner,
        ILogger<CourseFilteredReadRepository> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<PagedResult<Course>> GetFilteredAsync(CourseFilter filter, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Repo] GetFiltered(category={Category}, author={AuthorId}, search={Search}, page={Page}, size={Size})",
            filter.Category, filter.AuthorId, filter.Search, filter.Page, filter.PageSize);

        var result = await _inner.GetFilteredAsync(filter, cancellationToken);

        _logger.LogInformation("[Repo] GetFiltered -> {Count} of {Total}", result.Items.Count, result.TotalCount);

        return result;
    }
}