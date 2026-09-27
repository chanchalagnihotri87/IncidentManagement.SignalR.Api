using IncidentManagement.Domain.Entities;
using IncidentManagement.Domain.Enums;

namespace IncidentManagement.Application.Common.Interfaces;

/// <summary>
/// Persistence gateway for <see cref="Incident"/> records. Implemented by the infrastructure layer.
/// </summary>
public interface IIncidentRepository : IBaseRepository<Incident, Guid>
{
    Task<Incident?> GetByPublicIdAsync(Guid publicId, CancellationToken cancellationToken = default);

    /// <summary>Looks up an incident by its human-readable number (e.g. "INC-1001").</summary>
    Task<Incident?> GetByIncidentNumberAsync(string incidentNumber, CancellationToken cancellationToken = default);

    Task<bool> ExistsByIncidentNumberAsync(string incidentNumber, CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Incident>> GetByStatusAsync(IncidentStatus status, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Incident>> GetByAssigneeAsync(Guid assigneeId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Incident>> GetByTeamAsync(Guid teamId, CancellationToken cancellationToken = default);
}
