using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;


namespace OnlineCoursesApp.Monolith.WebApi.Services;
public class CurrentUserService(
    IHttpContextAccessor accessor) : ICurrentUserService
{
    public bool IsAuthenticated =>
        accessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public Guid UserId
    {
        get
        {
            var value = accessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                ?? accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(value) || !Guid.TryParse(value, out var id))
                throw new InvalidOperationException("User isn't authenticated");

            return id;
        }
    }

    public RoleName Role
    {
        get
        {
            var value = accessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrEmpty(value) || !Enum.TryParse<RoleName>(value, out var role))
                throw new InvalidOperationException("Role isn't found in the token.");

            return role;
        }
    }
}