using IncidentManagement.Application.Common.Interfaces;
using IncidentManagement.Application.Incidents.Models;
using IncidentManagement.Domain.Entities;
using IncidentManagement.Domain.Enums;

namespace IncidentManagement.Application.Incidents;

/// <summary>
/// Validates and persists new incidents, assigning a human-readable incident number
/// (e.g. "INC-1001") and defaulting a freshly raised incident to <see cref="IncidentStatus.New"/>.
/// </summary>
public class IncidentService : IIncidentService
{
    private const int FirstIncidentNumber = 1001;

    private readonly IIncidentRepository _incidentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly ITeamRepository _teamRepository;

    public IncidentService(
        IIncidentRepository incidentRepository,
        IUserRepository userRepository,
        IServiceRepository serviceRepository,
        ITeamRepository teamRepository)
    {
        _incidentRepository = incidentRepository;
        _userRepository = userRepository;
        _serviceRepository = serviceRepository;
        _teamRepository = teamRepository;
    }

    public async Task<CreateIncidentResult> CreateIncidentAsync(
        Guid reporterId,
        CreateIncidentRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return CreateIncidentResult.Failure("Title is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            return CreateIncidentResult.Failure("Description is required.");
        }

        var reporter = await _userRepository.GetByIdAsync(reporterId, cancellationToken);
        if (reporter is null)
        {
            return CreateIncidentResult.Failure("Reporter account could not be found.");
        }

        if (!await _serviceRepository.ExistsAsync(request.ServiceId, cancellationToken))
        {
            return CreateIncidentResult.Failure("The specified service does not exist.");
        }

        if (!await _teamRepository.ExistsAsync(request.TeamId, cancellationToken))
        {
            return CreateIncidentResult.Failure("The specified team does not exist.");
        }

        if (request.AssigneeId.HasValue
            && !await _userRepository.ExistsAsync(request.AssigneeId.Value, cancellationToken))
        {
            return CreateIncidentResult.Failure("The specified assignee does not exist.");
        }

        var incident = new Incident
        {
            PublicId = Guid.NewGuid(),
            IncidentNumber = await GenerateIncidentNumberAsync(cancellationToken),
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Severity = request.Severity,
            Priority = request.Priority,
            Status = IncidentStatus.New,
            Category = request.Category,
            Environment = request.Environment,
            ServiceId = request.ServiceId,
            ReporterId = reporterId,
            AssigneeId = request.AssigneeId,
            TeamId = request.TeamId
        };

        await _incidentRepository.AddAsync(incident, cancellationToken);

        var created = await _incidentRepository.GetByIdAsync(incident.Id, cancellationToken)
            ?? incident;

        return CreateIncidentResult.Success(IncidentDto.FromEntity(created));
    }

    public async Task<IReadOnlyList<IncidentDto>> GetAllIncidentsAsync(CancellationToken cancellationToken = default)
    {
        var incidents = await _incidentRepository.GetAllAsync(cancellationToken);
        return incidents.Select(IncidentDto.FromEntity).ToList();
    }

    public async Task<UpdateIncidentStatusResult> UpdateIncidentStatusAsync(
        Guid publicId,
        IncidentStatus status,
        CancellationToken cancellationToken = default)
    {
        var incident = await _incidentRepository.GetByPublicIdAsync(publicId, cancellationToken);
        if (incident is null)
        {
            return UpdateIncidentStatusResult.Failure("Incident could not be found.");
        }

        incident.Status = status;
        if (status == IncidentStatus.Resolved)
        {
            incident.ResolvedDate = DateTime.UtcNow;
        }

        await _incidentRepository.UpdateAsync(incident, cancellationToken);

        var updated = await _incidentRepository.GetByPublicIdAsync(publicId, cancellationToken)
            ?? incident;

        return UpdateIncidentStatusResult.Success(IncidentDto.FromEntity(updated));
    }

    /// <summary>
    /// Picks the next sequential "INC-####" number, re-checking for a collision so a race
    /// with another in-flight create cannot produce a duplicate.
    /// </summary>
    private async Task<string> GenerateIncidentNumberAsync(CancellationToken cancellationToken)
    {
        var sequence = FirstIncidentNumber + await _incidentRepository.CountAsync(cancellationToken);

        string candidate;
        do
        {
            candidate = $"INC-{sequence}";
            sequence++;
        } while (await _incidentRepository.ExistsByIncidentNumberAsync(candidate, cancellationToken));

        return candidate;
    }
}
