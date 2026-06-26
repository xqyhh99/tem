using BackendSystem.Application.Abstractions;
using BackendSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackendSystem.Infrastructure.Permissions;

/// <summary>
/// 权限查询（基于 EF Core 联表查询用户的角色与权限点）
/// </summary>
public class PermissionProvider : IPermissionProvider
{
    private readonly AppDbContext _db;

    public PermissionProvider(AppDbContext db)
    {
        _db = db;
    }

    /// <inheritdoc/>
    public async Task<IList<string>> GetRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _db.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .Join(_db.Roles,
                  ur => ur.RoleId,
                  r => r.Id,
                  (ur, r) => r.Name)
            .Distinct()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IList<string>> GetPermissionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _db.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .Join(_db.RolePermissions,
                  ur => ur.RoleId,
                  rp => rp.RoleId,
                  (ur, rp) => rp.PermissionId)
            .Join(_db.Permissions,
                  pid => pid,
                  p => p.Id,
                  (pid, p) => p.Code)
            .Distinct()
            .ToListAsync(cancellationToken);
    }
}
