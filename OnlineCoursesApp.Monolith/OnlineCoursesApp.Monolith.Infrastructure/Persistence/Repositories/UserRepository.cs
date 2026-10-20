using Microsoft.EntityFrameworkCore;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public Task<User?> GetByEmailAsync(string email, CancellationToken ct)
        => dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email, ct);      
    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
        => dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id, ct);
    public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
        => dbContext.Users.AnyAsync(u => u.Email == email, ct);
    public async Task AddAsync(User user, CancellationToken ct)
        => await dbContext.Users.AddAsync(user, ct);
    public void Remove(User user) => dbContext.Users.Remove(user);
}