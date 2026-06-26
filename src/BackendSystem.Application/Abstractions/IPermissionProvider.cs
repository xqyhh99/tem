namespace BackendSystem.Application.Abstractions;

/// <summary>
/// 权限查询抽象（由 Infrastructure 实现，查询用户的角色与权限点）
/// </summary>
public interface IPermissionProvider
{
    /// <summary>
    /// 获取用户的角色名称集合
    /// </summary>
    /// <param name="userId">用户 Id</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>角色名称集合</returns>
    Task<IList<string>> GetRolesAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取用户的权限点集合（如 user.create / user.read）
    /// </summary>
    /// <param name="userId">用户 Id</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>权限编码集合</returns>
    Task<IList<string>> GetPermissionsAsync(Guid userId, CancellationToken cancellationToken = default);
}
