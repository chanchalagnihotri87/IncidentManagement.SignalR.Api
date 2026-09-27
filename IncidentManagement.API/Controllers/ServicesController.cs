using IncidentManagement.Application.Common.Interfaces;
using IncidentManagement.Application.Services;
using IncidentManagement.Application.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IncidentManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ServicesController : ControllerBase
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IServiceCatalogService _serviceCatalogService;

    public ServicesController(IServiceRepository serviceRepository, IServiceCatalogService serviceCatalogService)
    {
        _serviceRepository = serviceRepository;
        _serviceCatalogService = serviceCatalogService;
    }

    /// <summary>Lists active services, for populating the "service" picker when raising an incident.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var services = await _serviceRepository.GetAllAsync(cancellationToken);

        var result = services
            .Where(s => s.IsActive)
            .OrderBy(s => s.Name)
            .Select(s => new { s.Id, s.Name });

        return Ok(result);
    }

    /// <summary>Registers a new service that incidents can be raised against.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateServiceRequest request, CancellationToken cancellationToken)
    {
        var result = await _serviceCatalogService.CreateServiceAsync(request, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(new { error = result.Error });
        }

        return Created($"api/services/{result.Service!.Id}", result.Service);
    }
}
