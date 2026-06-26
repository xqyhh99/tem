using BackendSystem.Application.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BackendSystem.Api.Controllers;

/// <summary>
/// 认证接口（登录 / 刷新 Token）
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="command">登录请求</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>AccessToken 与 RefreshToken</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// 刷新 Token（用 RefreshToken 换取新的 AccessToken）
    /// </summary>
    /// <param name="command">刷新请求</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>新的 AccessToken 与 RefreshToken</returns>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
