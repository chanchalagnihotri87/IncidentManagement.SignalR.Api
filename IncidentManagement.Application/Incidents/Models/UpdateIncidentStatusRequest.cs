using IncidentManagement.Domain.Enums;

namespace IncidentManagement.Application.Incidents.Models;

/// <summary>Request body for changing an incident's status.</summary>
public record UpdateIncidentStatusRequest(IncidentStatus Status);
