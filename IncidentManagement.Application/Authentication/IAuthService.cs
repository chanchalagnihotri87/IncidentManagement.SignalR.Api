using IncidentManagement.Application.Authentication.Models;

namespace IncidentManagement.Application.Authentication;

/// <summary>Authenticates users and issues JWT access tokens.</summary>
public interface IAuthService
{
    Task<LoginResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
