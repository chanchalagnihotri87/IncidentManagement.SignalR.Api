using IncidentManagement.Application.Teams.Models;

namespace IncidentManagement.Application.Teams;

/// <summary>Application-level operations for managing a team's membership.</summary>
public interface ITeamMemberService
{
    Task<AddTeamMemberResult> AddMemberAsync(Guid teamId, AddTeamMemberRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TeamMemberDto>> GetMembersAsync(Guid teamId, CancellationToken cancellationToken = default);
}
