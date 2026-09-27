using IncidentManagement.Application.Common.Interfaces;
using IncidentManagement.Domain.Entities;
using IncidentManagement.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace IncidentManagement.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core-backed <see cref="IUserRepository"/> against IncidentManagementDB.
/// </summary>
public class UserRepository : BaseRepository<User, Guid>, IUserRepository
{
    public UserRepository(IncidentManagementDbContext dbContext) : base(dbContext)
    {
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        DbSet.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower(), cancellationToken);

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        DbSet.AnyAsync(u => u.Email.ToLower() == email.ToLower(), cancellationToken);

    public override async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await DbSet.AsNoTracking().OrderBy(u => u.FullName).ToListAsync(cancellationToken);
}
