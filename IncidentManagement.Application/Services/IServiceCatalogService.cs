using IncidentManagement.Application.Services.Models;

namespace IncidentManagement.Application.Services;

/// <summary>Application-level operations for managing the catalog of services incidents can be raised against.</summary>
public interface IServiceCatalogService
{
    Task<CreateServiceResult> CreateServiceAsync(CreateServiceRequest request, CancellationToken cancellationToken = default);
}
