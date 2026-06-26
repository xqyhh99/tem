namespace BackendSystem.Domain.Exceptions;

/// <summary>
/// 业务逻辑异常（用户不存在 / 密码错误等）
/// </summary>
public class BusinessException : Exception
{
    /// <summary>业务错误码</summary>
    public int StatusCode { get; }

    /// <summary>
    /// 构造业务异常
    /// </summary>
    /// <param name="message">异常信息</param>
    /// <param name="statusCode">HTTP 状态码，默认 400</param>
    public BusinessException(string message, int statusCode = 400)
        : base(message)
    {
        StatusCode = statusCode;
    }
}
