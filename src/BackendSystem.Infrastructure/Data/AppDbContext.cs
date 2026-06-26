using BackendSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendSystem.Infrastructure.Data;

/// <summary>
/// EF Core 数据上下文（RBAC 标准结构）
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>用户表</summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>角色表</summary>
    public DbSet<Role> Roles => Set<Role>();

    /// <summary>权限点表</summary>
    public DbSet<Permission> Permissions => Set<Permission>();

    /// <summary>菜单表</summary>
    public DbSet<Menu> Menus => Set<Menu>();

    /// <summary>RefreshToken 表</summary>
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    /// <summary>用户-角色关联表</summary>
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    /// <summary>角色-权限关联表</summary>
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 用户
        modelBuilder.Entity<User>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Username).HasMaxLength(64).IsRequired();
            b.HasIndex(x => x.Username).IsUnique();
            b.Property(x => x.PasswordHash).HasMaxLength(256).IsRequired();
        });

        // 角色
        modelBuilder.Entity<Role>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).HasMaxLength(64).IsRequired();
        });

        // 权限点
        modelBuilder.Entity<Permission>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Code).HasMaxLength(128).IsRequired();
            b.HasIndex(x => x.Code).IsUnique();
            b.Property(x => x.Description).HasMaxLength(256);
        });

        // 菜单
        modelBuilder.Entity<Menu>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).HasMaxLength(64).IsRequired();
            b.Property(x => x.Path).HasMaxLength(256);
            b.Property(x => x.PermissionCode).HasMaxLength(128);
        });

        // RefreshToken
        modelBuilder.Entity<RefreshToken>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Token).HasMaxLength(128).IsRequired();
            b.HasIndex(x => x.UserId);
        });

        // 用户-角色关联
        modelBuilder.Entity<UserRole>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasOne(x => x.User).WithMany(u => u.UserRoles).HasForeignKey(x => x.UserId);
            b.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId);
            b.HasIndex(x => new { x.UserId, x.RoleId }).IsUnique();
        });

        // 角色-权限关联
        modelBuilder.Entity<RolePermission>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasOne(x => x.Role).WithMany(r => r.RolePermissions).HasForeignKey(x => x.RoleId);
            b.HasOne(x => x.Permission).WithMany().HasForeignKey(x => x.PermissionId);
            b.HasIndex(x => new { x.RoleId, x.PermissionId }).IsUnique();
        });
    }
}
