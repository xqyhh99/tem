using BackendSystem.Application.Abstractions;
using BackendSystem.Domain.Entities;
using BackendSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackendSystem.Infrastructure.Repositories;

/// <summary>
/// 用户仓储（EF Core 实现）
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    /// <inheritdoc/>
    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }
}
