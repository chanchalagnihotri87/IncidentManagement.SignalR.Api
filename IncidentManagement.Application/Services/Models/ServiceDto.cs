using IncidentManagement.Domain.Entities;

namespace IncidentManagement.Application.Services.Models;

/// <summary>Read-facing projection of a <see cref="Service"/>.</summary>
public record ServiceDto(Guid Id, string Name, string? Description, bool IsActive, DateTime CreatedAtUtc)
{
    public static ServiceDto FromEntity(Service service) => new(
        service.Id,
        service.Name,
        service.Description,
        service.IsActive,
        service.CreatedAtUtc);
}
