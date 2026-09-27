using IncidentManagement.Application.Common.Interfaces;
using IncidentManagement.Domain.Entities;
using IncidentManagement.Domain.Enums;
using IncidentManagement.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace IncidentManagement.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core-backed <see cref="IIncidentRepository"/> against IncidentManagementDB.
/// </summary>
public class IncidentRepository : BaseRepository<Incident, Guid>, IIncidentRepository
{
    public IncidentRepository(IncidentManagementDbContext dbContext) : base(dbContext)
    {
    }

    private IQueryable<Incident> IncidentsWithDetails() =>
        DbSet
            .Include(i => i.Service)
            .Include(i => i.Team)
            .Include(i => i.Reporter)
            .Include(i => i.Assignee);

    public override Task<Incident?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        IncidentsWithDetails().FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

    public Task<Incident?> GetByPublicIdAsync(Guid publicId, CancellationToken cancellationToken = default) =>
        IncidentsWithDetails().FirstOrDefaultAsync(i => i.PublicId == publicId, cancellationToken);

    public Task<Incident?> GetByIncidentNumberAsync(string incidentNumber, CancellationToken cancellationToken = default) =>
        IncidentsWithDetails().FirstOrDefaultAsync(i => i.IncidentNumber == incidentNumber, cancellationToken);

    public Task<bool> ExistsByIncidentNumberAsync(string incidentNumber, CancellationToken cancellationToken = default) =>
        DbSet.AnyAsync(i => i.IncidentNumber == incidentNumber, cancellationToken);

    public Task<int> CountAsync(CancellationToken cancellationToken = default) =>
        DbSet.CountAsync(cancellationToken);

    public override async Task<IReadOnlyList<Incident>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await IncidentsWithDetails().AsNoTracking()
            .OrderByDescending(i => i.CreatedAtUtc)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Incident>> GetByStatusAsync(IncidentStatus status, CancellationToken cancellationToken = default) =>
        await IncidentsWithDetails().AsNoTracking()
            .Where(i => i.Status == status)
            .OrderByDescending(i => i.CreatedAtUtc)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Incident>> GetByAssigneeAsync(Guid assigneeId, CancellationToken cancellationToken = default) =>
        await IncidentsWithDetails().AsNoTracking()
            .Where(i => i.AssigneeId == assigneeId)
            .OrderByDescending(i => i.CreatedAtUtc)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Incident>> GetByTeamAsync(Guid teamId, CancellationToken cancellationToken = default) =>
        await IncidentsWithDetails().AsNoTracking()
            .Where(i => i.TeamId == teamId)
            .OrderByDescending(i => i.CreatedAtUtc)
            .ToListAsync(cancellationToken);
}
