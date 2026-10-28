namespace OnlineCoursesApp.Monolith.Domain.Repositories;

public interface IRepository<T> where T : class
{
    Task AddAsync(T entity, CancellationToken cancellation);

    void Remove(T entity);
}