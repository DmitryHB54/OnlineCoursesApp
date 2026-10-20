using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Application.Auth;
public interface IRefreshTokenService
{
    string GenerateRaw();
    string Hash(string raw);
    Task<(string Raw, RefreshToken Entity)> CreateAsync(Guid userId, CancellationToken ct);
    Task<RefreshToken?> GetActiveByRawAsync(string raw, CancellationToken ct);
    Task RevokeAsync(RefreshToken token, string? replacedByHash, CancellationToken ct);
}