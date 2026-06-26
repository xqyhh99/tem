namespace BackendSystem.Domain.Entities;

/// <summary>
/// 用户-角色关联表（RBAC 多对多中间表）
/// </summary>
public class UserRole
{
    /// <summary>主键</summary>
    public Guid Id { get; set; }

    /// <summary>用户 Id</summary>
    public Guid UserId { get; set; }

    /// <summary>角色 Id</summary>
    public Guid RoleId { get; set; }

    /// <summary>导航属性：用户</summary>
    public User User { get; set; } = null!;

    /// <summary>导航属性：角色</summary>
    public Role Role { get; set; } = null!;
}
