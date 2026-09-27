using IncidentManagement.Domain.Entities;

namespace IncidentManagement.Application.Common.Interfaces;

/// <summary>
/// Persistence gateway for <see cref="Service"/> records. Implemented by the infrastructure layer.
/// </summary>
public interface IServiceRepository : IBaseRepository<Service, Guid>
{
    /// <summary>Checks whether a service with this name already exists. Matching is case-insensitive.</summary>
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
}
