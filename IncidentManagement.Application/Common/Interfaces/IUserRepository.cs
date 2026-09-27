using IncidentManagement.Domain.Entities;

namespace IncidentManagement.Application.Common.Interfaces;

/// <summary>
/// Persistence gateway for <see cref="User"/> accounts. Implemented by the infrastructure layer.
/// </summary>
public interface IUserRepository : IBaseRepository<User, Guid>
{
    /// <summary>Looks up a user by login e-mail. Matching is case-insensitive.</summary>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
}
