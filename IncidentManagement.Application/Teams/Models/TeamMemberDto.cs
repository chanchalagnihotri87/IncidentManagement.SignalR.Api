using IncidentManagement.Domain.Entities;
using IncidentManagement.Domain.Enums;

namespace IncidentManagement.Application.Teams.Models;

/// <summary>Read-facing projection of a <see cref="TeamMember"/>.</summary>
public record TeamMemberDto(Guid Id, Guid TeamId, Guid UserId, string UserFullName, TeamMemberRole Role, bool IsActive, DateTime CreatedAtUtc)
{
    public static TeamMemberDto FromEntity(TeamMember member) => new(
        member.Id,
        member.TeamId,
        member.UserId,
        member.User.FullName,
        member.Role,
        member.IsActive,
        member.CreatedAtUtc);
}
