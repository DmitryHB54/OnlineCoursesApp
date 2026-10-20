using Microsoft.EntityFrameworkCore;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Domain.Exceptions;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Repositories
{
    public class RoleRepository(AppDbContext dbContext) : IRoleRepository
    {
        public Task<Role?> GetByNameAsync(RoleName name, CancellationToken ct) =>
            dbContext.Roles.FirstOrDefaultAsync(r => r.Name == name, ct);

        public async Task<Role> GetRequiredByNameAsync(RoleName name, CancellationToken ct)
        {
            var role = await GetByNameAsync(name, ct);
            if (role is null)
                throw new NotFoundException($"Role '{name}' not found into the data base.");
            return role;
        }
    }
}