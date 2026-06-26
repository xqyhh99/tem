namespace BackendSystem.Domain.Entities;

/// <summary>
/// 权限点（精细到按钮 / 接口）
/// </summary>
public class Permission
{
    /// <summary>权限主键</summary>
    public Guid Id { get; set; }

    /// <summary>权限编码，例如 user.create / user.delete</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>权限描述</summary>
    public string Description { get; set; } = string.Empty;
}
