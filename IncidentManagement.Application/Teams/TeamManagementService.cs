using IncidentManagement.Application.Common.Interfaces;
using IncidentManagement.Application.Teams.Models;
using IncidentManagement.Domain.Entities;

namespace IncidentManagement.Application.Teams;

/// <summary>Validates and persists new teams, guarding against duplicate names.</summary>
public class TeamManagementService : ITeamManagementService
{
    private readonly ITeamRepository _teamRepository;

    public TeamManagementService(ITeamRepository teamRepository)
    {
        _teamRepository = teamRepository;
    }

    public async Task<CreateTeamResult> CreateTeamAsync(
        CreateTeamRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return CreateTeamResult.Failure("Name is required.");
        }

        var name = request.Name.Trim();

        if (await _teamRepository.ExistsByNameAsync(name, cancellationToken))
        {
            return CreateTeamResult.Failure("A team with this name already exists.");
        }

        var team = new Team
        {
            Name = name,
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim()
        };

        await _teamRepository.AddAsync(team, cancellationToken);

        return CreateTeamResult.Success(TeamDto.FromEntity(team));
    }
}
