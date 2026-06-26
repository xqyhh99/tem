using BackendSystem.Application.Abstractions;

namespace BackendSystem.Infrastructure.Auth;

/// <summary>
/// 密码哈希（基于 BCrypt，生产级安全存储）
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    /// <inheritdoc/>
    public string Hash(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
    }

    /// <inheritdoc/>
    public bool Verify(string password, string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
