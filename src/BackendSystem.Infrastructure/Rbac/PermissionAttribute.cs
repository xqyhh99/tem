namespace BackendSystem.Infrastructure.Rbac;

/// <summary>
/// 权限控制特性（用于接口/控制器，声明所需权限点）
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public class PermissionAttribute : Attribute
{
    /// <summary>所需权限编码，例如 user.create</summary>
    public string Code { get; }

    public PermissionAttribute(string code)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        Code = code;
    }
}
