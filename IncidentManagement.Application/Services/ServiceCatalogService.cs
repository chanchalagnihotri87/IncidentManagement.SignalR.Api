using IncidentManagement.Application.Common.Interfaces;
using IncidentManagement.Application.Services.Models;
using IncidentManagement.Domain.Entities;

namespace IncidentManagement.Application.Services;

/// <summary>Validates and persists new services, guarding against duplicate names.</summary>
public class ServiceCatalogService : IServiceCatalogService
{
    private readonly IServiceRepository _serviceRepository;

    public ServiceCatalogService(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<CreateServiceResult> CreateServiceAsync(
        CreateServiceRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return CreateServiceResult.Failure("Name is required.");
        }

        var name = request.Name.Trim();

        if (await _serviceRepository.ExistsByNameAsync(name, cancellationToken))
        {
            return CreateServiceResult.Failure("A service with this name already exists.");
        }

        var service = new Service
        {
            Name = name,
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim()
        };

        await _serviceRepository.AddAsync(service, cancellationToken);

        return CreateServiceResult.Success(ServiceDto.FromEntity(service));
    }
}
