using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineCoursesApp.Monolith.Application.Auth.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Domain.Exceptions;

namespace OnlineCoursesApp.Monolith.Application.Auth.Commands.Login;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher<User> passwordHasher,
    IJwtTokenService jwtTokenService,
    IRefreshTokenService refreshTokenService) : IRequestHandler<LoginCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await userRepository.GetByEmailAsync(email, cancellationToken);
        if (user is null)
            throw new ForbiddenException("Password or email is invlaid");

        var verification = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (verification == PasswordVerificationResult.Failed)
            throw new ForbiddenException("Password or email is invlaid");

        var (accessToken, accessExpiresAt) = jwtTokenService.Generate(user);
        var (rawRefresh, refreshEntity) = await refreshTokenService.CreateAsync(user.Id, cancellationToken);

        return new AuthResponseDto(
            accessToken,
            accessExpiresAt,
            rawRefresh,
            refreshEntity.ExpiresAt);
    }
}