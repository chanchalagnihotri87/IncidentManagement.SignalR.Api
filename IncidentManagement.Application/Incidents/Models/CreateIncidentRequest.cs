using IncidentManagement.Domain.Enums;

namespace IncidentManagement.Application.Incidents.Models;

/// <summary>Payload for raising a new incident. The reporter is taken from the caller's JWT, not this body.</summary>
public record CreateIncidentRequest(
    string Title,
    string Description,
    IncidentSeverity Severity,
    IncidentPriority Priority,
    IncidentCategory Category,
    IncidentEnvironment Environment,
    Guid ServiceId,
    Guid TeamId,
    Guid? AssigneeId);
