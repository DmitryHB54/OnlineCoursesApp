using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Domain.Repositories;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Decorators;

public class CachedCourseRepository : ICourseRepository
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);
    private static string Key(Guid id) => $"course:{id}";

    private readonly ICourseRepository _inner;
    private readonly IMemoryCache _cache;
    private readonly ILogger<ICourseRepository> _logger;

    public CachedCourseRepository(
        ICourseRepository inner,
        IMemoryCache cache,
        ILogger<ICourseRepository> logger)
    {
        _inner = inner;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        if(_cache.TryGetValue(Key(id), out Course? cached) && cached is not null)
        {
            _logger.LogInformation("[Cache] HIT course:{Id}", id);
            return cached;
        }

        _logger.LogInformation("[Cache] MISS course:{Id}", id);
        var course = await _inner.GetByIdAsync(id, cancellationToken);

        if (course is not null)
        {
            _cache.Set(Key(id), course, CacheTtl);
        }

        return course;
    }

    public Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _inner.ExistsByIdAsync(id, cancellationToken);
    }

    public Task<Course?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken)
    { 
        return _inner.GetByIdForUpdateAsync(id, cancellationToken);    
    }

    public async Task AddAsync(Course course, CancellationToken ct)
    {
        _cache.Remove(Key(course.Id));

        await _inner.AddAsync(course, ct);
    }

    public void Update(Course course)
    {
        _cache.Remove(Key(course.Id));

        _logger.LogInformation("[Cache] INVALIDATE course:{Id} (Update)", course.Id);

        _inner.Update(course);
    }

    public void Remove(Course course)
    {
        _cache.Remove(Key(course.Id));

        _logger.LogInformation("[Cache] INVALIDATE course:{Id} (Remove)", course.Id);

        _inner.Remove(course);
    }

}