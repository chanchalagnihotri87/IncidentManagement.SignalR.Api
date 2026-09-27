using IncidentManagement.Domain.Entities;
using IncidentManagement.Domain.Enums;

namespace IncidentManagement.Application.Incidents.Models;

/// <summary>Read-facing projection of an <see cref="Incident"/>.</summary>
public record IncidentDto(
    Guid PublicId,
    string IncidentNumber,
    string Title,
    string Description,
    IncidentSeverity Severity,
    IncidentPriority Priority,
    IncidentStatus Status,
    IncidentCategory Category,
    IncidentEnvironment Environment,
    Guid ServiceId,
    string ServiceName,
    Guid TeamId,
    string TeamName,
    Guid ReporterId,
    string ReporterName,
    Guid? AssigneeId,
    string? AssigneeName,
    DateTime CreatedAtUtc)
{
    public static IncidentDto FromEntity(Incident incident) => new(
        incident.PublicId,
        incident.IncidentNumber,
        incident.Title,
        incident.Description,
        incident.Severity,
        incident.Priority,
        incident.Status,
        incident.Category,
        incident.Environment,
        incident.ServiceId,
        incident.Service.Name,
        incident.TeamId,
        incident.Team.Name,
        incident.ReporterId,
        incident.Reporter.FullName,
        incident.AssigneeId,
        incident.Assignee?.FullName,
        incident.CreatedAtUtc);
}
