using IncidentManagement.Application.Common.Interfaces;
using IncidentManagement.Application.Teams.Models;
using IncidentManagement.Domain.Entities;

namespace IncidentManagement.Application.Teams;

/// <summary>Validates and persists team membership changes.</summary>
public class TeamMemberService : ITeamMemberService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;

    public TeamMemberService(
        ITeamRepository teamRepository,
        IUserRepository userRepository,
        ITeamMemberRepository teamMemberRepository)
    {
        _teamRepository = teamRepository;
        _userRepository = userRepository;
        _teamMemberRepository = teamMemberRepository;
    }

    public async Task<AddTeamMemberResult> AddMemberAsync(
        Guid teamId,
        AddTeamMemberRequest request,
        CancellationToken cancellationToken = default)
    {
        var team = await _teamRepository.GetByIdAsync(teamId, cancellationToken);
        if (team is null)
        {
            return AddTeamMemberResult.Failure("Team not found.");
        }

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return AddTeamMemberResult.Failure("User not found.");
        }

        var existingMembership = await _teamMemberRepository.GetByTeamAndUserAsync(teamId, request.UserId, cancellationToken);

        TeamMember member;

        if (existingMembership is not null)
        {
            if (existingMembership.IsActive)
            {
                return AddTeamMemberResult.Failure("User is already a member of this team.");
            }

            existingMembership.IsActive = true;
            existingMembership.Role = request.Role;
            await _teamMemberRepository.UpdateAsync(existingMembership, cancellationToken);
            member = existingMembership;
        }
        else
        {
            member = new TeamMember
            {
                TeamId = teamId,
                UserId = request.UserId,
                Role = request.Role,
                IsActive = true
            };

            await _teamMemberRepository.AddAsync(member, cancellationToken);
        }

        member.User = user;

        return AddTeamMemberResult.Success(TeamMemberDto.FromEntity(member));
    }

    public async Task<IReadOnlyList<TeamMemberDto>> GetMembersAsync(Guid teamId, CancellationToken cancellationToken = default)
    {
        var members = await _teamMemberRepository.GetByTeamAsync(teamId, cancellationToken);

        return members.Select(TeamMemberDto.FromEntity).ToList();
    }
}
