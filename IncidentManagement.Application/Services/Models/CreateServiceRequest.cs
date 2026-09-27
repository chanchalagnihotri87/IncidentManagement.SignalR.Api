namespace IncidentManagement.Application.Services.Models;

/// <summary>Payload for registering a new service that incidents can be raised against.</summary>
public record CreateServiceRequest(string Name, string? Description);
