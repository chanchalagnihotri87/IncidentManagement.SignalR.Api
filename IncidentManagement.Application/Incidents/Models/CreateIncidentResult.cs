namespace IncidentManagement.Application.Incidents.Models;

/// <summary>Outcome of a create-incident attempt. On success carries the created incident.</summary>
public record CreateIncidentResult(bool Succeeded, IncidentDto? Incident, string? Error)
{
    public static CreateIncidentResult Success(IncidentDto incident) => new(true, incident, null);

    public static CreateIncidentResult Failure(string error) => new(false, null, error);
}
