using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Application.Auth;

public interface IJwtTokenService
{
    (string Token, DateTime ExpiresAt) Generate(User user);
}