using BackendSystem.Application.Abstractions;
using BackendSystem.Domain.Exceptions;
using MediatR;

namespace BackendSystem.Application.Auth.Commands;

/// <summary>
/// 刷新 Token 逻辑：校验 RefreshToken 有效后签发新的 AccessToken，并轮换 RefreshToken
/// </summary>
public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, LoginResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPermissionProvider _permissionProvider;
    private readonly IJwtService _jwt;
    private readonly IRefreshTokenService _refresh;

    public RefreshTokenHandler(
        IUserRepository userRepository,
        IPermissionProvider permissionProvider,
        IJwtService jwt,
        IRefreshTokenService refresh)
    {
        _userRepository = userRepository;
        _permissionProvider = permissionProvider;
        _jwt = jwt;
        _refresh = refresh;
    }

    public async Task<LoginResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // 取出 Redis 中存储的 RefreshToken
        var stored = await _refresh.GetAsync(request.UserId);
        if (string.IsNullOrEmpty(stored) || !stored.Equals(request.RefreshToken, StringComparison.Ordinal))
        {
            throw new BusinessException("RefreshToken 无效或已过期", 401);
        }

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null || !user.IsActive)
        {
            throw new BusinessException("用户不存在或已被禁用", 401);
        }

        // 获取角色与权限点
        var roles = await _permissionProvider.GetRolesAsync(user.Id, cancellationToken);
        var permissions = await _permissionProvider.GetPermissionsAsync(user.Id, cancellationToken);

        // 签发新的 AccessToken
        var accessToken = _jwt.CreateToken(user, roles, permissions);

        // 轮换 RefreshToken（旧的失效，颁发新的）
        var newRefreshToken = Guid.NewGuid().ToString("N");
        await _refresh.SaveAsync(user.Id, newRefreshToken);

        return new LoginResult
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken
        };
    }
}
