namespace IncidentManagement.Application.Teams.Models;

/// <summary>Outcome of a create-team attempt. On success carries the created team.</summary>
public record CreateTeamResult(bool Succeeded, TeamDto? Team, string? Error)
{
    public static CreateTeamResult Success(TeamDto team) => new(true, team, null);

    public static CreateTeamResult Failure(string error) => new(false, null, error);
}
