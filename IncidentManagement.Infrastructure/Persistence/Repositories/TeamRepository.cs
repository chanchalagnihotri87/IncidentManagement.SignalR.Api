using IncidentManagement.Application.Common.Interfaces;
using IncidentManagement.Domain.Entities;
using IncidentManagement.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace IncidentManagement.Infrastructure.Persistence.Repositories;

/// <summary>EF Core-backed <see cref="ITeamRepository"/> against IncidentManagementDB.</summary>
public class TeamRepository : BaseRepository<Team, Guid>, ITeamRepository
{
    public TeamRepository(IncidentManagementDbContext dbContext) : base(dbContext)
    {
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default) =>
        DbSet.AnyAsync(t => t.Name.ToLower() == name.ToLower(), cancellationToken);
}
