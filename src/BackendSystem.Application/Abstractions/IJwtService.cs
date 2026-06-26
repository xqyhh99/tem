using BackendSystem.Domain.Entities;

namespace BackendSystem.Application.Abstractions;

/// <summary>
/// JWT 生成服务抽象（由 Infrastructure 实现）
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// 生成 AccessToken
    /// </summary>
    /// <param name="user">登录用户</param>
    /// <param name="roles">用户角色集合</param>
    /// <param name="permissions">用户权限点集合</param>
    /// <returns>JWT AccessToken 字符串</returns>
    string CreateToken(User user, IList<string> roles, IList<string> permissions);
}
