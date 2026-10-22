using Microsoft.Extensions.Logging;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Application.Models;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Infrastructure.Persistence.Repositories;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Decorators;

public class LoggingCourseRepository(
    ICourseRepository inner,
    ILogger<CourseRepository> logger) : ICourseRepository
{
    public async Task<Course?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        logger.LogInformation("[Repo] GetById({id})", id);
        var result = await inner.GetByIdAsync(id, ct);
        logger.LogInformation("[Repo] GetById({Id}) -> {Status}",
            id, result is null ? "not found" : "found");
        return result; 
    }
    public async Task<Course?> GetByIdForUpdateAsync(Guid id, CancellationToken ct)
    {
        logger.LogInformation("[Repo] GetByIdForUpdateAsync({id})", id);
        return await inner.GetByIdForUpdateAsync(id, ct);
    }
    public async Task<PagedResult<Course>> GetFilteredAsync(CourseFilter filter, CancellationToken ct)
    {
        logger.LogInformation("[Repo] GetFiltered(category={Category}, author={AuthorId}, search={Search}, page={Page}, size={Size})",
            filter.Category, filter.AuthorId, filter.Search, filter.Page, filter.PageSize);
        var result = await inner.GetFilteredAsync(filter, ct);
        logger.LogInformation("[Repo] GetFiltered -> {Count} of {Total}", result.Items.Count, result.TotalCount);
        return result;
    }

    public async Task AddAsync(Course course, CancellationToken ct)
    {
        logger.LogInformation("[Repo] Add course '{Title}' ({Id})", course.Title, course.Id);
        await inner.AddAsync(course, ct);
    }
    public void Update(Course course)
    {
        logger.LogInformation("[Repo] Update course {Id}", course.Id);
        inner.Update(course);
    }

    public void Remove(Course course)
    {
        logger.LogInformation("[Repo] Remove course {Id}", course.Id);
        inner.Remove(course);
    }
}