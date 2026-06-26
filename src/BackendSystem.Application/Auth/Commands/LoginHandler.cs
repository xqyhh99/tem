using BackendSystem.Application.Abstractions;
using BackendSystem.Domain.Exceptions;
using MediatR;

namespace BackendSystem.Application.Auth.Commands;

/// <summary>
/// 登录逻辑（核心）
/// </summary>
public class LoginHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPermissionProvider _permissionProvider;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwt;
    private readonly IRefreshTokenService _refresh;

    public LoginHandler(
        IUserRepository userRepository,
        IPermissionProvider permissionProvider,
        IPasswordHasher passwordHasher,
        IJwtService jwt,
        IRefreshTokenService refresh)
    {
        _userRepository = userRepository;
        _permissionProvider = permissionProvider;
        _passwordHasher = passwordHasher;
        _jwt = jwt;
        _refresh = refresh;
    }

    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 参数校验
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Username);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Password);

        // 查用户
        var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (user is null)
        {
            throw new BusinessException("用户不存在");
        }

        if (!user.IsActive)
        {
            throw new BusinessException("该账号已被禁用", 403);
        }

        // 密码校验（生产必须 Hash 对比，不能明文）
        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new BusinessException("密码错误", 401);
        }

        // 获取角色与权限点
        var roles = await _permissionProvider.GetRolesAsync(user.Id, cancellationToken);
        var permissions = await _permissionProvider.GetPermissionsAsync(user.Id, cancellationToken);

        // 生成 AccessToken
        var accessToken = _jwt.CreateToken(user, roles, permissions);

        // 生成 RefreshToken 并存入 Redis
        var refreshToken = Guid.NewGuid().ToString("N");
        await _refresh.SaveAsync(user.Id, refreshToken);

        return new LoginResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}
