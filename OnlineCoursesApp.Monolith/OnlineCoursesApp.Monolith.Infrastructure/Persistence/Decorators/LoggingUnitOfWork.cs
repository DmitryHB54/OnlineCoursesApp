using Microsoft.Extensions.Logging;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using System.Reflection;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Decorators;

public class LoggingUnitOfWork(
    IUnitOfWork inner,
    ILogger<UnitOfWork> logger) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        var result = await inner.SaveChangesAsync(ct);
        logger.LogInformation("[Repo] SaveChanges committed");
        return result;
    }
}