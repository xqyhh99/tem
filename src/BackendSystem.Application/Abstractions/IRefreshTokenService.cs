namespace BackendSystem.Application.Abstractions;

/// <summary>
/// RefreshToken 管理抽象（由 Infrastructure 基于 Redis 实现）
/// </summary>
public interface IRefreshTokenService
{
    /// <summary>
    /// 保存 RefreshToken
    /// </summary>
    /// <param name="userId">用户 Id</param>
    /// <param name="token">RefreshToken 值</param>
    /// <returns>表示异步操作的任务</returns>
    Task SaveAsync(Guid userId, string token);

    /// <summary>
    /// 获取 RefreshToken
    /// </summary>
    /// <param name="userId">用户 Id</param>
    /// <returns>RefreshToken 值；不存在时返回 null</returns>
    Task<string?> GetAsync(Guid userId);

    /// <summary>
    /// 删除 RefreshToken（登出 / 踢人）
    /// </summary>
    /// <param name="userId">用户 Id</param>
    /// <returns>表示异步操作的任务</returns>
    Task DeleteAsync(Guid userId);
}
