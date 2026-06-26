namespace BackendSystem.Domain.Entities;

/// <summary>
/// 角色-权限关联表（RBAC 多对多中间表）
/// </summary>
public class RolePermission
{
    /// <summary>主键</summary>
    public Guid Id { get; set; }

    /// <summary>角色 Id</summary>
    public Guid RoleId { get; set; }

    /// <summary>权限 Id</summary>
    public Guid PermissionId { get; set; }

    /// <summary>导航属性：角色</summary>
    public Role Role { get; set; } = null!;

    /// <summary>导航属性：权限</summary>
    public Permission Permission { get; set; } = null!;
}
