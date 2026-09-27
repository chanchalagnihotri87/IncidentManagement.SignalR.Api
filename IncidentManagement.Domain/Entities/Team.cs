using IncidentManagement.Domain.Common;

namespace IncidentManagement.Domain.Entities;

/// <summary>
/// A group of users that incidents can be assigned to for triage and resolution.
/// </summary>
public class Team : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();
}
