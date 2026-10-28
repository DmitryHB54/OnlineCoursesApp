using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Domain.Enums;

namespace OnlineCoursesApp.Monolith.Domain.Repositories;

public interface IRoleRepository
{
    Task<Role?> GetByNameAsync(RoleName name, CancellationToken cancellationToken);

    Task<Role> GetRequiredByNameAsync(RoleName name, CancellationToken cancellationToken);
}