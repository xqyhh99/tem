namespace BackendSystem.Application.Auth.Commands;

/// <summary>
/// 登录返回结果
/// </summary>
public class LoginResult
{
    /// <summary>JWT AccessToken</summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>RefreshToken（用于续期）</summary>
    public string RefreshToken { get; set; } = string.Empty;
}
