using IncidentManagement.Application.Common.Interfaces;
using IncidentManagement.Application.Teams;
using IncidentManagement.Application.Teams.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IncidentManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeamsController : ControllerBase
{
    private readonly ITeamRepository _teamRepository;
    private readonly ITeamManagementService _teamManagementService;
    private readonly ITeamMemberService _teamMemberService;

    public TeamsController(
        ITeamRepository teamRepository,
        ITeamManagementService teamManagementService,
        ITeamMemberService teamMemberService)
    {
        _teamRepository = teamRepository;
        _teamManagementService = teamManagementService;
        _teamMemberService = teamMemberService;
    }

    /// <summary>Lists active teams, for populating the "team" picker when raising an incident.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var teams = await _teamRepository.GetAllAsync(cancellationToken);

        var result = teams
            .Where(t => t.IsActive)
            .OrderBy(t => t.Name)
            .Select(t => new { t.Id, t.Name });

        return Ok(result);
    }

    /// <summary>Registers a new team that incidents can be assigned to.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTeamRequest request, CancellationToken cancellationToken)
    {
        var result = await _teamManagementService.CreateTeamAsync(request, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(new { error = result.Error });
        }

        return Created($"api/teams/{result.Team!.Id}", result.Team);
    }

    /// <summary>Adds a user to a team, or reactivates their membership if they were previously removed.</summary>
    [HttpPost("{teamId:guid}/members")]
    public async Task<IActionResult> AddMember(Guid teamId, [FromBody] AddTeamMemberRequest request, CancellationToken cancellationToken)
    {
        var result = await _teamMemberService.AddMemberAsync(teamId, request, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(new { error = result.Error });
        }

        return Created($"api/teams/{teamId}/members", result.Member);
    }

    /// <summary>Lists the members of a team.</summary>
    [HttpGet("{teamId:guid}/members")]
    public async Task<IActionResult> GetMembers(Guid teamId, CancellationToken cancellationToken)
    {
        var members = await _teamMemberService.GetMembersAsync(teamId, cancellationToken);

        return Ok(members);
    }
}
