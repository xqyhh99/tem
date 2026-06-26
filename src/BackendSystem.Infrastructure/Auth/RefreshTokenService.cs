using BackendSystem.Application.Abstractions;
using Microsoft.Extensions.Caching.Distributed;

namespace BackendSystem.Infrastructure.Auth;

/// <summary>
/// RefreshToken 管理（基于 Redis / IDistributedCache 实现）
/// </summary>
public class RefreshTokenService : IRefreshTokenService
{
    private readonly IDistributedCache _cache;

    public RefreshTokenService(IDistributedCache cache)
    {
        _cache = cache;
    }

    /// <inheritdoc/>
    public async Task SaveAsync(Guid userId, string token)
    {
        await _cache.SetStringAsync(
            $"refresh:{userId}",
            token,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7) // RefreshToken 7 天有效
            });
    }

    /// <inheritdoc/>
    public async Task<string?> GetAsync(Guid userId)
    {
        return await _cache.GetStringAsync($"refresh:{userId}");
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(Guid userId)
    {
        await _cache.RemoveAsync($"refresh:{userId}");
    }
}
