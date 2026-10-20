using Microsoft.EntityFrameworkCore;
using OnlineCoursesApp.Monolith.Application.Auth;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Infrastructure.Persistence;
using System.Security.Cryptography;
using System.Text;

namespace OnlineCoursesApp.Monolith.Infrastructure.Auth;

public class RefreshTokenService(
    AppDbContext dbContext,
    IUnitOfWork unitOfWork) : IRefreshTokenService
{
    public string GenerateRaw()
    {
        byte[] bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
    public string Hash(string raw)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(raw));
        return Convert.ToHexString(bytes);
    }
    public async Task<(string Raw, RefreshToken Entity)> CreateAsync(Guid userId, CancellationToken ct)
    {
        var raw = GenerateRaw();

        var token = new RefreshToken()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TokenHash = Hash(raw),
            CreatedAt = DateTimeOffset.UtcNow,
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(7)
        };

        dbContext.RefreshTokens.Add(token);
        await unitOfWork.SaveChangesAsync(ct);

        return (raw, token);
    }

    public Task<RefreshToken?> GetActiveByRawAsync(string raw, CancellationToken ct)
    {
        var hash = Hash(raw);
        return dbContext.RefreshTokens
            .Include(t => t.User).ThenInclude(u => u.Role)
            .FirstOrDefaultAsync(t => t.TokenHash == hash &&
            !t.IsRevoked && t.ExpiresAt > DateTimeOffset.UtcNow, ct);
    }

    public async Task RevokeAsync(RefreshToken token, string? replacedByHash, CancellationToken ct)
    {
        token.IsRevoked = true;
        token.RevokedAt = DateTimeOffset.UtcNow;
        token.ReplacedByTokenHash = replacedByHash;
        await unitOfWork.SaveChangesAsync(ct);
    }
}