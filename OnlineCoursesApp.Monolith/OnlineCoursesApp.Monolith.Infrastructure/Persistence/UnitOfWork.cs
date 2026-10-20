namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence;
public class UnitOfWork(AppDbContext dbContext)
{
    public Task<int> SaveChangesAsync(CancellationToken ct)
        => dbContext.SaveChangesAsync(ct);
}