using Microsoft.Extensions.Logging;
using OnlineCoursesApp.Monolith.Domain.Repositories;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Decorators;

public class LoggingUnitOfWork : IUnitOfWork
{
    private readonly IUnitOfWork _inner;
    private readonly ILogger<UnitOfWork> _logger;

    public LoggingUnitOfWork(
        IUnitOfWork inner,
        ILogger<UnitOfWork> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        var result = await _inner.SaveChangesAsync(ct);

        _logger.LogInformation("[Repo] SaveChanges committed");

        return result;
    }
}