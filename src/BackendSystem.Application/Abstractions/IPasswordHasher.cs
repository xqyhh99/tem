namespace BackendSystem.Application.Abstractions;

/// <summary>
/// 密码哈希抽象（由 Infrastructure 实现，生产环境必须使用 BCrypt / PBKDF2 等）
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// 对明文密码进行哈希
    /// </summary>
    /// <param name="password">明文密码</param>
    /// <returns>哈希后的密码</returns>
    string Hash(string password);

    /// <summary>
    /// 校验明文密码与哈希是否匹配
    /// </summary>
    /// <param name="password">明文密码</param>
    /// <param name="passwordHash">已存储的哈希</param>
    /// <returns>匹配返回 true；否则返回 false</returns>
    bool Verify(string password, string passwordHash);
}
