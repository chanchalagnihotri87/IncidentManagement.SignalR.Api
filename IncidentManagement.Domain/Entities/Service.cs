using IncidentManagement.Domain.Common;

namespace IncidentManagement.Domain.Entities;

/// <summary>
/// A system or application that incidents can be raised against.
/// </summary>
public class Service : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}
