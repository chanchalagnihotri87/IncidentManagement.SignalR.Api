using IncidentManagement.Domain.Entities;

namespace IncidentManagement.Application.Common.Interfaces;

/// <summary>
/// Persistence gateway for <see cref="TeamMember"/> records. Implemented by the infrastructure layer.
/// </summary>
public interface ITeamMemberRepository : IBaseRepository<TeamMember, Guid>
{
    /// <summary>Looks up a single team/user membership row, active or not.</summary>
    Task<TeamMember?> GetByTeamAndUserAsync(Guid teamId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>Lists every membership row for a team, with the member's <see cref="User"/> loaded.</summary>
    Task<IReadOnlyList<TeamMember>> GetByTeamAsync(Guid teamId, CancellationToken cancellationToken = default);
}
