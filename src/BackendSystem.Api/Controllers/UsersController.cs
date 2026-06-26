using BackendSystem.Infrastructure.Rbac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendSystem.Api.Controllers;

/// <summary>
/// 用户管理接口（演示 RBAC 权限控制）
/// </summary>
[ApiController]
[Route("api/users")]
[Authorize] // 整个控制器需要登录
public class UsersController : ControllerBase
{
    /// <summary>
    /// 创建用户（需要 user.create 权限）
    /// </summary>
    [Permission("user.create")]
    [HttpPost]
    public IActionResult CreateUser()
    {
        return Ok("通过权限验证，创建用户成功");
    }

    /// <summary>
    /// 查询用户（需要 user.read 权限）
    /// </summary>
    [Permission("user.read")]
    [HttpGet]
    public IActionResult GetUsers()
    {
        return Ok("通过权限验证，返回用户列表");
    }
}
