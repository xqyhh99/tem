using BackendSystem.Application.Abstractions;
using BackendSystem.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BackendSystem.Infrastructure.Data;

/// <summary>
/// 数据库种子数据（初始化测试数据）
/// 系统启动时自动运行，确保数据库中有可用的测试账号
/// </summary>
public static class SeedData
{
    // 固定的 ID，确保关联表可以正确引用
    private static readonly Guid AdminUserId = new Guid("11111111-1111-1111-1111-111111111111");
    private static readonly Guid AdminRoleId = new Guid("22222222-2222-2222-2222-222222222222");
    private static readonly Guid UserRoleId = new Guid("33333333-3333-3333-3333-333333333333");
    
    private static readonly Guid PermissionCreateId = new Guid("44444444-4444-4444-4444-444444444444");
    private static readonly Guid PermissionReadId = new Guid("55555555-5555-5555-5555-555555555555");
    private static readonly Guid PermissionDeleteId = new Guid("66666666-6666-6666-6666-666666666666");

    /// <summary>
    /// 初始化数据库种子数据
    /// </summary>
    /// <param name="app">Web 应用实例</param>
    /// <param name="password">管理员初始密码</param>
    public static async Task InitializeAsync(this IApplicationBuilder app, string password = "123456")
    {
        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        await db.Database.MigrateAsync();

        await SeedUsersAsync(db, passwordHasher, password);
        await SeedRolesAsync(db);
        await SeedPermissionsAsync(db);
        await SeedUserRolesAsync(db);
        await SeedRolePermissionsAsync(db);
        await SeedMenusAsync(db);
    }

    private static async Task SeedUsersAsync(AppDbContext db, IPasswordHasher hasher, string password)
    {
        if (!await db.Users.AnyAsync())
        {
            db.Users.AddRange(new[]
            {
                new User
                {
                    Id = AdminUserId,
                    Username = "admin",
                    PasswordHash = hasher.Hash(password),
                    IsActive = true
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Username = "testuser",
                    PasswordHash = hasher.Hash(password),
                    IsActive = true
                }
            });
            await db.SaveChangesAsync();
        }
    }

    private static async Task SeedRolesAsync(AppDbContext db)
    {
        if (!await db.Roles.AnyAsync())
        {
            db.Roles.AddRange(new[]
            {
                new Role { Id = AdminRoleId, Name = "Admin" },
                new Role { Id = UserRoleId, Name = "User" }
            });
            await db.SaveChangesAsync();
        }
    }

    private static async Task SeedPermissionsAsync(AppDbContext db)
    {
        if (!await db.Permissions.AnyAsync())
        {
            db.Permissions.AddRange(new[]
            {
                new Permission { Id = PermissionCreateId, Code = "user.create", Description = "创建用户" },
                new Permission { Id = PermissionReadId, Code = "user.read", Description = "查看用户" },
                new Permission { Id = PermissionDeleteId, Code = "user.delete", Description = "删除用户" }
            });
            await db.SaveChangesAsync();
        }
    }

    private static async Task SeedUserRolesAsync(AppDbContext db)
    {
        if (!await db.UserRoles.AnyAsync())
        {
            db.UserRoles.AddRange(new[]
            {
                new UserRole { Id = Guid.NewGuid(), UserId = AdminUserId, RoleId = AdminRoleId },
                new UserRole { Id = Guid.NewGuid(), UserId = AdminUserId, RoleId = UserRoleId }
            });
            await db.SaveChangesAsync();
        }
    }

    private static async Task SeedRolePermissionsAsync(AppDbContext db)
    {
        if (!await db.RolePermissions.AnyAsync())
        {
            db.RolePermissions.AddRange(new[]
            {
                new RolePermission { Id = Guid.NewGuid(), RoleId = AdminRoleId, PermissionId = PermissionCreateId },
                new RolePermission { Id = Guid.NewGuid(), RoleId = AdminRoleId, PermissionId = PermissionReadId },
                new RolePermission { Id = Guid.NewGuid(), RoleId = AdminRoleId, PermissionId = PermissionDeleteId },
                new RolePermission { Id = Guid.NewGuid(), RoleId = UserRoleId, PermissionId = PermissionReadId }
            });
            await db.SaveChangesAsync();
        }
    }

    private static async Task SeedMenusAsync(AppDbContext db)
    {
        if (!await db.Menus.AnyAsync())
        {
            db.Menus.AddRange(new[]
            {
                new Menu { Id = Guid.NewGuid(), Name = "用户管理", Path = "/users", PermissionCode = "user.read" },
                new Menu { Id = Guid.NewGuid(), Name = "角色管理", Path = "/roles", PermissionCode = "user.read" },
                new Menu { Id = Guid.NewGuid(), Name = "权限管理", Path = "/permissions", PermissionCode = "user.read" }
            });
            await db.SaveChangesAsync();
        }
    }
}
