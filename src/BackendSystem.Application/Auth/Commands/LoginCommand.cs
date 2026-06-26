using MediatR;

namespace BackendSystem.Application.Auth.Commands;

/// <summary>
/// 登录请求
/// </summary>
/// <param name="Username">用户名</param>
/// <param name="Password">密码（明文，由服务端校验 Hash）</param>
public record LoginCommand(string Username, string Password) : IRequest<LoginResult>;
