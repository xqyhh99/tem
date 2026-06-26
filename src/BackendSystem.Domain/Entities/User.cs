namespace BackendSystem.Domain.Entities;

/// <summary>
/// 用户实体（系统核心）
/// </summary>
public class User
{
    /// <summary>用户主键</summary>
    public Guid Id { get; set; }

    /// <summary>用户名（登录账号）</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>密码（必须加密存储，不能明文）</summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>是否启用</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>用户与角色的关联集合</summary>
    public List<UserRole> UserRoles { get; set; } = new();
}
