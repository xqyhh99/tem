namespace BackendSystem.Domain.Entities;

/// <summary>
/// 角色实体（管理员 / 普通用户）
/// </summary>
public class Role
{
    /// <summary>角色主键</summary>
    public Guid Id { get; set; }

    /// <summary>角色名称，例如 Admin / User</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>角色与权限的关联集合</summary>
    public List<RolePermission> RolePermissions { get; set; } = new();
}
