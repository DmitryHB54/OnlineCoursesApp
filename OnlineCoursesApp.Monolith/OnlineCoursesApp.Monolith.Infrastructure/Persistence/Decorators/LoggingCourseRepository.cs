using Microsoft.Extensions.Logging;
using OnlineCoursesApp.Monolith.Application.Models;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Domain.Repositories;
using OnlineCoursesApp.Monolith.Infrastructure.Persistence.Repositories;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Decorators;

public class LoggingCourseRepository : ICourseRepository
{
    private readonly ICourseRepository _inner;
    private readonly ILogger<CourseRepository> _logger;

    public LoggingCourseRepository(
        ICourseRepository inner,
        ILogger<CourseRepository> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Repo] GetById({id})", id);

        var result = await _inner.GetByIdAsync(id, cancellationToken);

        _logger.LogInformation("[Repo] GetById({Id}) -> {Status}",
            id, result is null ? "not found" : "found");

        return result; 
    }

    public async Task<Course?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Repo] GetByIdForUpdateAsync({id})", id);

        return await _inner.GetByIdForUpdateAsync(id, cancellationToken);
    }

    public async Task AddAsync(Course course, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Repo] Add course '{Title}' ({Id})", course.Title, course.Id);

        await _inner.AddAsync(course, cancellationToken);
    }

    public void Update(Course course)
    {
        _logger.LogInformation("[Repo] Update course {Id}", course.Id);

        _inner.Update(course);
    }

    public void Remove(Course course)
    {
        _logger.LogInformation("[Repo] Remove course {Id}", course.Id);

        _inner.Remove(course);
    }
}