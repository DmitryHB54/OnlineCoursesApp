using MediatR;
using OnlineCoursesApp.Monolith.Application.Auth.Dtos;
using OnlineCoursesApp.Monolith.Domain.Exceptions;

namespace OnlineCoursesApp.Monolith.Application.Auth.Commands.Refresh;

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, AuthResponseDto>
{
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshCommandHandler(
        IRefreshTokenService refreshTokenService,
        IJwtTokenService jwtTokenService)
    {
        _refreshTokenService = refreshTokenService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponseDto> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var existing = await _refreshTokenService
            .GetActiveByRawAsync(request.RefreshToken, cancellationToken);

        if (existing is null)
        {
            throw new ForbiddenException("Refresh token is invalid or expired.");
        }

        var (newRaw, newEntity) = await _refreshTokenService.CreateAsync(existing.UserId, cancellationToken);
        await _refreshTokenService.RevokeAsync(existing, newEntity.TokenHash, cancellationToken);

        var (accessToken, accessExpiresAt) = _jwtTokenService.Generate(existing.User);

        return new AuthResponseDto(
            accessToken,
            accessExpiresAt,
            newRaw,
            newEntity.ExpiresAt);
    }
}