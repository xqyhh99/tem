using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BackendSystem.Infrastructure.Rbac;

/// <summary>
/// 权限解析与校验中间件（RBAC 核心）
/// 根据接口上标注的 <see cref="PermissionAttribute"/> 校验当前用户的权限点
/// </summary>
public class PermissionMiddleware
{
    private readonly RequestDelegate _next;

    public PermissionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // 从 JWT 解析权限点，放入 HttpContext.Items 供后续使用
        var permClaim = context.User.FindFirst("perm")?.Value;
        if (!string.IsNullOrEmpty(permClaim))
        {
            context.Items["permissions"] = permClaim.Split(',', StringSplitOptions.RemoveEmptyEntries);
        }

        // 查找当前终结点是否标注了 PermissionAttribute
        var endpoint = context.GetEndpoint();
        var required = endpoint?.Metadata.GetMetadata<PermissionAttribute>();
        if (required is null)
        {
            // 无权限要求，直接放行
            await _next(context);
            return;
        }

        // 未登录直接拒绝
        if (context.User.Identity?.IsAuthenticated != true)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("未登录");
            return;
        }

        // 校验权限点是否包含所需 Code
        var userPermissions = context.Items["permissions"] as string[] ?? Array.Empty<string>();
        if (!userPermissions.Contains(required.Code, StringComparer.Ordinal))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync($"权限不足，需要权限：{required.Code}");
            return;
        }

        await _next(context);
    }
}
