using System.Net;
using System.Text.Json;
using BackendSystem.Domain.Exceptions;

namespace BackendSystem.Api.Middlewares;

/// <summary>
/// 全局异常处理中间件：将业务异常转换为统一的 JSON 错误响应
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessException ex)
        {
            // 业务异常：按 StatusCode 返回，不记录为错误
            await WriteAsync(context, ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "未处理的异常");
            await WriteAsync(context, (int)HttpStatusCode.InternalServerError, "服务器内部错误");
        }
    }

    private static Task WriteAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var payload = JsonSerializer.Serialize(new { code = statusCode, message });
        return context.Response.WriteAsync(payload);
    }
}
