using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;

namespace OnlineCoursesApp.Monolith.WebApi.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _accessor;

    public CurrentUserService(
        IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public bool IsAuthenticated
    {
        get
        {
            return _accessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
        }
    }

    public Guid UserId
    {
        get
        {
            var value = _accessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                ?? _accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(value) || !Guid.TryParse(value, out var id))
                throw new InvalidOperationException("User isn't authenticated");

            return id;
        }
    }

    public RoleName Role
    {
        get
        {
            var value = _accessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrEmpty(value) || !Enum.TryParse<RoleName>(value, out var role))
                throw new InvalidOperationException("Role isn't found in the token.");

            return role;
        }
    }
}