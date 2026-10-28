using Microsoft.EntityFrameworkCore;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Domain.Exceptions;
using OnlineCoursesApp.Monolith.Domain.Repositories;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Repositories;
public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _dbContext;

    public RoleRepository(
        AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Role?> GetByNameAsync(RoleName name, CancellationToken cancellationToken)
    { 
        return _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }

    public async Task<Role> GetRequiredByNameAsync(RoleName name, CancellationToken cancellationToken)
    {
        var role = await GetByNameAsync(name, cancellationToken);
        if (role is null)
        {
            throw new NotFoundException($"Role '{name}' not found into the data base.");
        }

        return role;
    }
}