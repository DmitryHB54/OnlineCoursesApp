using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineCoursesApp.Monolith.Application.Auth.Dtos;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Domain.Exceptions;
using OnlineCoursesApp.Monolith.Domain.Repositories;

namespace OnlineCoursesApp.Monolith.Application.Auth.Commands.Login;

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenService _refreshTokenService;

    public LoginCommandHandler(IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher<User> passwordHasher,
        IJwtTokenService jwtTokenService,
        IRefreshTokenService refreshTokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _refreshTokenService = refreshTokenService;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        if (user is null)
        {
            throw new ForbiddenException("Password or email is invlaid");
        }
            
        var verification = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (verification == PasswordVerificationResult.Failed)
        { 
            throw new ForbiddenException("Password or email is invlaid");
        }

        var (accessToken, accessExpiresAt) = _jwtTokenService.Generate(user);
        var (rawRefresh, refreshEntity) = await _refreshTokenService.CreateAsync(user.Id, cancellationToken);

        return new AuthResponseDto(
            accessToken,
            accessExpiresAt,
            rawRefresh,
            refreshEntity.ExpiresAt);
    }
}