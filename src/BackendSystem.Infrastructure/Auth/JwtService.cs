using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackendSystem.Application.Abstractions;
using BackendSystem.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BackendSystem.Infrastructure.Auth;

/// <summary>
/// JWT 生成服务（负责签发 AccessToken）
/// </summary>
public class JwtService : IJwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    /// <inheritdoc/>
    public string CreateToken(User user, IList<string> roles, IList<string> permissions)
    {
        // Claims = Token 携带的信息
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()), // 用户 ID
            new(ClaimTypes.Name, user.Username),                // 用户名
        };

        // 角色 Claim（支持多角色）
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // 权限点 Claim（合并为一个逗号分隔的字符串）
        if (permissions.Count > 0)
        {
            claims.Add(new Claim("perm", string.Join(",", permissions)));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new InvalidOperationException("未配置 Jwt:Key")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // AccessToken 短期有效
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
