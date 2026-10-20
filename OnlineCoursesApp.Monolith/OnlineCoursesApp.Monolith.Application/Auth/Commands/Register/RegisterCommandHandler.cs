using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineCoursesApp.Monolith.Application.Auth.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Domain.Exceptions;

namespace OnlineCoursesApp.Monolith.Application.Auth.Commands.Register;

public class RegisterCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepositiry,
    IUnitOfWork unitOfWork,
    IPasswordHasher<User> passwordHasher,
    IJwtTokenService jwtTokenService,
    IRefreshTokenService refreshTokenService) : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        if (await userRepository.ExistsByEmailAsync(email, cancellationToken))
            throw new ForbiddenException("User with the same email already exists.");

        var role = await roleRepositiry.GetRequiredByNameAsync(request.Role, cancellationToken);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            FullName = request.FullName.Trim(),
            RoleId = role.Id,
            Role = role,
            CreatedAt = DateTime.UtcNow
        };
        user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var (accessToken, accessExpiresAt) = jwtTokenService.Generate(user);
        var (rawRefresh, refreshEntity) = await refreshTokenService.CreateAsync(user.Id, cancellationToken);

        return new AuthResponseDto(
            accessToken,
            accessExpiresAt,
            rawRefresh,
            refreshEntity.ExpiresAt);
    }
}