using MediatR;
using OnlineCoursesApp.Monolith.Application.Auth.Dtos;
using OnlineCoursesApp.Monolith.Domain.Exceptions;

namespace OnlineCoursesApp.Monolith.Application.Auth.Commands.Refresh;
public class RefreshCommandHandler(
    IRefreshTokenService refreshTokenService,
    IJwtTokenService jwtTokenService) : IRequestHandler<RefreshCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var existing = await refreshTokenService
            .GetActiveByRawAsync(request.RefreshToken, cancellationToken);

        if (existing is null)
            throw new ForbiddenException("Refresh token is invalid or expired.");

        var (newRaw, newEntity) = await refreshTokenService.CreateAsync(existing.UserId, cancellationToken);
        await refreshTokenService.RevokeAsync(existing, newEntity.TokenHash, cancellationToken);

        var (accessToken, accessExpiresAt) = jwtTokenService.Generate(existing.User);

        return new AuthResponseDto(
            accessToken,
            accessExpiresAt,
            newRaw,
            newEntity.ExpiresAt);
    }
}