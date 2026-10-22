using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Application.Models;
using OnlineCoursesApp.Monolith.Domain.Entities;
using System.Reflection;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Decorators;
public class CachedCourseRepository(
    ICourseRepository inner,
    IMemoryCache cache,
    ILogger<ICourseRepository> logger) : ICourseRepository
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);
    private static string Key(Guid id) => $"course:{id}";

    public async Task<Course?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        if(cache.TryGetValue(Key(id), out Course? cached) && cached is not null)
        {
            logger.LogInformation("[Cache] HIT course:{Id}", id);
            return cached;
        }

        logger.LogInformation("[Cache] MISS course:{Id}", id);
        var course = await inner.GetByIdAsync(id, ct);

        if (course is not null)
            cache.Set(Key(id), course, CacheTtl);

        return course;
    }
    public Task<Course?> GetByIdForUpdateAsync(Guid id, CancellationToken ct)
            => inner.GetByIdForUpdateAsync(id, ct);

    public Task<PagedResult<Course>> GetFilteredAsync(CourseFilter filter, CancellationToken ct)
        => inner.GetFilteredAsync(filter, ct);

    public async Task AddAsync(Course course, CancellationToken ct)
    {
        cache.Remove(Key(course.Id));
        await inner.AddAsync(course, ct);
    }
    public void Update(Course course)
    {
        cache.Remove(Key(course.Id));
        logger.LogInformation("[Cache] INVALIDATE course:{Id} (Update)", course.Id);
        inner.Update(course);
    }

    public void Remove(Course course)
    {
        cache.Remove(Key(course.Id));
        logger.LogInformation("[Cache] INVALIDATE course:{Id} (Remove)", course.Id);
        inner.Remove(course);
    }
}