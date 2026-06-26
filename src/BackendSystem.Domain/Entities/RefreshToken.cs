namespace BackendSystem.Domain.Entities;

/// <summary>
/// RefreshToken（用于续期登录）
/// </summary>
public class RefreshToken
{
    /// <summary>主键</summary>
    public Guid Id { get; set; }

    /// <summary>所属用户 Id</summary>
    public Guid UserId { get; set; }

    /// <summary>RefreshToken 值</summary>
    public string Token { get; set; } = string.Empty;
    //测试
    /// <summary>过期时间</summary>
    public DateTime ExpireAt { get; set; }

    /// <summary>是否已撤销</summary>
    public bool Revoked { get; set; }
}
