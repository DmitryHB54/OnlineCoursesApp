using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Application.Auth;

public interface IRefreshTokenService
{
    string GenerateRaw();

    string Hash(string raw);

    Task<(string Raw, RefreshToken Entity)> CreateAsync(Guid userId, CancellationToken cancellationToken);

    Task<RefreshToken?> GetActiveByRawAsync(string raw, CancellationToken cancellationToken);

    Task RevokeAsync(RefreshToken token, string? replacedByHash, CancellationToken cancellationToken);
}