using IncidentManagement.Domain.Enums;

namespace IncidentManagement.Application.Teams.Models;

/// <summary>Payload for adding a user to a team. The team is identified by the route, not this body.</summary>
public record AddTeamMemberRequest(Guid UserId, TeamMemberRole Role);
