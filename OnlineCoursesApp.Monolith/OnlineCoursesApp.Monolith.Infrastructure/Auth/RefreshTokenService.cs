using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesApp.Monolith.Application.Auth;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Infrastructure.Persistence;
using OnlineCoursesApp.Monolith.Domain.Repositories;

namespace OnlineCoursesApp.Monolith.Infrastructure.Auth;

public class RefreshTokenService
    : IRefreshTokenService
{
    private readonly AppDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenService(
        AppDbContext dbContext,
        IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

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

    public async Task<(string Raw, RefreshToken Entity)> CreateAsync(Guid userId, CancellationToken cancellationToken)
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

        _dbContext.RefreshTokens.Add(token);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return (raw, token);
    }

    public Task<RefreshToken?> GetActiveByRawAsync(string raw, CancellationToken cancellationToken)
    {
        var hash = Hash(raw);

        return _dbContext.RefreshTokens
            .Include(t => t.User).ThenInclude(u => u.Role)
            .FirstOrDefaultAsync(t => t.TokenHash == hash &&
            !t.IsRevoked && t.ExpiresAt > DateTimeOffset.UtcNow, cancellationToken);
    }

    public async Task RevokeAsync(RefreshToken token, string? replacedByHash, CancellationToken cancellationToken)
    {
        token.IsRevoked = true;
        token.RevokedAt = DateTimeOffset.UtcNow;
        token.ReplacedByTokenHash = replacedByHash;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}