using BackendSystem.Application.Abstractions;
using BackendSystem.Infrastructure.Auth;
using BackendSystem.Infrastructure.Data;
using BackendSystem.Infrastructure.Permissions;
using BackendSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BackendSystem.Infrastructure;

/// <summary>
/// Infrastructure 层依赖注入注册扩展
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// 注册 Infrastructure 层服务：EF Core、Redis、JWT、RBAC 仓储等
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <returns>服务集合（便于链式调用）</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core（SQL Server）
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Redis 分布式缓存（用于 RefreshToken / 登录状态）
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "BackendSystem:";
        });

        // 应用层抽象 -> Infrastructure 实现
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPermissionProvider, PermissionProvider>();
        services.AddSingleton<IJwtService, JwtService>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();

        return services;
    }
}
