using System.Security.Claims;
using IncidentManagement.API.Hubs;
using IncidentManagement.Application.Incidents;
using IncidentManagement.Application.Incidents.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace IncidentManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IncidentsController : ControllerBase
{
    private readonly IIncidentService _incidentService;
    private readonly IHubContext<IncidentHub> _incidentHub;

    public IncidentsController(IIncidentService incidentService, IHubContext<IncidentHub> incidentHub)
    {
        _incidentService = incidentService;
        _incidentHub = incidentHub;
    }

    /// <summary>Lists all incidents, newest first.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var incidents = await _incidentService.GetAllIncidentsAsync(cancellationToken);
        return Ok(incidents);
    }

    /// <summary>Raises a new incident, reported by the current authenticated user.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateIncidentRequest request, CancellationToken cancellationToken)
    {
        var reporterIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(reporterIdClaim, out var reporterId))
        {
            return Unauthorized();
        }

        var result = await _incidentService.CreateIncidentAsync(reporterId, request, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(new { error = result.Error });
        }

        await _incidentHub.Clients.Group(IncidentHub.DashboardGroup).SendAsync(
            "IncidentCreated",
            result.Incident,
            cancellationToken);

        return Created($"api/incidents/{result.Incident!.PublicId}", result.Incident);
    }

    /// <summary>Changes an incident's status. Administrator only; broadcasts the change to the incident room.</summary>
    [HttpPatch("{publicId:guid}/status")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateStatus(
        Guid publicId,
        [FromBody] UpdateIncidentStatusRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _incidentService.UpdateIncidentStatusAsync(publicId, request.Status, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(new { error = result.Error });
        }

        var changedBy = User.FindFirstValue(ClaimTypes.Name);
        var changedAtUtc = DateTime.UtcNow;

        await _incidentHub.Clients.Group($"incident-{publicId}").SendAsync(
            "IncidentStatusChanged",
            new
            {
                incidentPublicId = publicId,
                status = result.Incident!.Status,
                changedBy,
                changedAtUtc
            },
            cancellationToken);

        // The dashboard gets the full incident so it can refresh the row without refetching.
        await _incidentHub.Clients.Group(IncidentHub.DashboardGroup).SendAsync(
            "DashboardIncidentStatusChanged",
            new
            {
                incident = result.Incident,
                changedBy,
                changedAtUtc
            },
            cancellationToken);

        return Ok(result.Incident);
    }
}
