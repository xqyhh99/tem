using System.Text;
using BackendSystem.Api.Middlewares;
using BackendSystem.Application;
using BackendSystem.Infrastructure;
using BackendSystem.Infrastructure.Data;
using BackendSystem.Infrastructure.Rbac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// ===== 服务注册 =====

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// MediatR（CQRS）
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationMarker).Assembly));

// Infrastructure（EF Core / Redis / JWT / RBAC 仓储）
builder.Services.AddInfrastructure(builder.Configuration);

// JWT 认证
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// 授权
builder.Services.AddAuthorization();

var app = builder.Build();

// ===== 数据库初始化（自动创建表和测试数据）=====
await app.InitializeAsync();

// ===== 中间件管道 =====

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 全局异常处理（最先执行，捕获所有后续异常）
app.UseMiddleware<ExceptionHandlingMiddleware>();

// 认证（必须） -> RBAC 权限中间件 -> 授权
app.UseAuthentication();
app.UseMiddleware<PermissionMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
