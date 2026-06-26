using BackendSystem.Domain.Entities;

namespace BackendSystem.Application.Abstractions;

/// <summary>
/// 用户仓储抽象（由 Infrastructure 基于 EF Core 实现）
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// 根据用户名查询用户（含角色与权限）
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>用户实体；不存在时返回 null</returns>
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据用户 Id 查询用户
    /// </summary>
    /// <param name="id">用户 Id</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>用户实体；不存在时返回 null</returns>
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
