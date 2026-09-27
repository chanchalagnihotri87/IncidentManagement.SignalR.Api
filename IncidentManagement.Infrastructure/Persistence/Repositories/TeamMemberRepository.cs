using IncidentManagement.Application.Common.Interfaces;
using IncidentManagement.Domain.Entities;
using IncidentManagement.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace IncidentManagement.Infrastructure.Persistence.Repositories;

/// <summary>EF Core-backed <see cref="ITeamMemberRepository"/> against IncidentManagementDB.</summary>
public class TeamMemberRepository : BaseRepository<TeamMember, Guid>, ITeamMemberRepository
{
    public TeamMemberRepository(IncidentManagementDbContext dbContext) : base(dbContext)
    {
    }

    public Task<TeamMember?> GetByTeamAndUserAsync(Guid teamId, Guid userId, CancellationToken cancellationToken = default) =>
        DbSet.FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.UserId == userId, cancellationToken);

    public async Task<IReadOnlyList<TeamMember>> GetByTeamAsync(Guid teamId, CancellationToken cancellationToken = default) =>
        await DbSet
            .Where(tm => tm.TeamId == teamId)
            .Include(tm => tm.User)
            .AsNoTracking()
            .OrderBy(tm => tm.User.FullName)
            .ToListAsync(cancellationToken);
}
