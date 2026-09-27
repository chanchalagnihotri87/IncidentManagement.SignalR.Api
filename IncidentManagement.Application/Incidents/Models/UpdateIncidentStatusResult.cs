namespace IncidentManagement.Application.Incidents.Models;

/// <summary>Outcome of a status-change attempt. On success carries the updated incident.</summary>
public record UpdateIncidentStatusResult(bool Succeeded, IncidentDto? Incident, string? Error)
{
    public static UpdateIncidentStatusResult Success(IncidentDto incident) => new(true, incident, null);

    public static UpdateIncidentStatusResult Failure(string error) => new(false, null, error);
}
