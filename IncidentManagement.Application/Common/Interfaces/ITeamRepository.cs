using IncidentManagement.Domain.Entities;

namespace IncidentManagement.Application.Common.Interfaces;

/// <summary>
/// Persistence gateway for <see cref="Team"/> records. Implemented by the infrastructure layer.
/// </summary>
public interface ITeamRepository : IBaseRepository<Team, Guid>
{
    /// <summary>Checks whether a team with this name already exists. Matching is case-insensitive.</summary>
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
}
