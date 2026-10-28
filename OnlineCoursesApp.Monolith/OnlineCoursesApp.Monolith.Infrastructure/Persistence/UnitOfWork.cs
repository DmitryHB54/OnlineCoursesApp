using OnlineCoursesApp.Monolith.Application.Common.Interfaces;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    { 
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}