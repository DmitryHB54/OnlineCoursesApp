using OnlineCoursesApp.Monolith.Domain.Enums;

namespace OnlineCoursesApp.Monolith.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }
    RoleName Role { get; }
    bool IsAuthenticated { get; }
}