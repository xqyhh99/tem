namespace BackendSystem.Domain.Entities;

/// <summary>
/// 菜单（后台左侧菜单）
/// </summary>
public class Menu
{
    /// <summary>菜单主键</summary>
    public Guid Id { get; set; }

    /// <summary>菜单名称</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>前端路由路径</summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>关联的权限编码</summary>
    public string PermissionCode { get; set; } = string.Empty;
}
