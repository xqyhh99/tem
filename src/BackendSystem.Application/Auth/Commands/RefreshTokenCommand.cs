using MediatR;

namespace BackendSystem.Application.Auth.Commands;

/// <summary>
/// 刷新 Token 请求（用 RefreshToken 换取新的 AccessToken）
/// </summary>
/// <param name="UserId">用户 Id</param>
/// <param name="RefreshToken">客户端持有的 RefreshToken</param>
public record RefreshTokenCommand(Guid UserId, string RefreshToken) : IRequest<LoginResult>;
