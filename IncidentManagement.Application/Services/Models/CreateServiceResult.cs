namespace IncidentManagement.Application.Services.Models;

/// <summary>Outcome of a create-service attempt. On success carries the created service.</summary>
public record CreateServiceResult(bool Succeeded, ServiceDto? Service, string? Error)
{
    public static CreateServiceResult Success(ServiceDto service) => new(true, service, null);

    public static CreateServiceResult Failure(string error) => new(false, null, error);
}
