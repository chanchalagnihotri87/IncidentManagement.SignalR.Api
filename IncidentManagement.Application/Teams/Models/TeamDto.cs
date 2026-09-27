using IncidentManagement.Domain.Entities;

namespace IncidentManagement.Application.Teams.Models;

/// <summary>Read-facing projection of a <see cref="Team"/>.</summary>
public record TeamDto(Guid Id, string Name, string? Description, bool IsActive, DateTime CreatedAtUtc)
{
    public static TeamDto FromEntity(Team team) => new(
        team.Id,
        team.Name,
        team.Description,
        team.IsActive,
        team.CreatedAtUtc);
}
