namespace IncidentManagement.Application.Teams.Models;

/// <summary>Payload for registering a new team that incidents can be assigned to.</summary>
public record CreateTeamRequest(string Name, string? Description);
