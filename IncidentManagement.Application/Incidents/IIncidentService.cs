using IncidentManagement.Application.Incidents.Models;

namespace IncidentManagement.Application.Incidents;

/// <summary>Application-level operations for raising and managing incidents.</summary>
public interface IIncidentService
{
    Task<CreateIncidentResult> CreateIncidentAsync(
        Guid reporterId,
        CreateIncidentRequest request,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<IncidentDto>> GetAllIncidentsAsync(CancellationToken cancellationToken = default);

    Task<UpdateIncidentStatusResult> UpdateIncidentStatusAsync(
        Guid publicId,
        IncidentManagement.Domain.Enums.IncidentStatus status,
        CancellationToken cancellationToken = default);
}
