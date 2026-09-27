using IncidentManagement.Application.Teams.Models;

namespace IncidentManagement.Application.Teams;

/// <summary>Application-level operations for managing the catalog of teams incidents can be assigned to.</summary>
public interface ITeamManagementService
{
    Task<CreateTeamResult> CreateTeamAsync(CreateTeamRequest request, CancellationToken cancellationToken = default);
}
