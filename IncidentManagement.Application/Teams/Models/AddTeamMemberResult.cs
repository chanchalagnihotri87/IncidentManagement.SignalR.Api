namespace IncidentManagement.Application.Teams.Models;

/// <summary>Outcome of an add-team-member attempt. On success carries the resulting membership.</summary>
public record AddTeamMemberResult(bool Succeeded, TeamMemberDto? Member, string? Error)
{
    public static AddTeamMemberResult Success(TeamMemberDto member) => new(true, member, null);

    public static AddTeamMemberResult Failure(string error) => new(false, null, error);
}
