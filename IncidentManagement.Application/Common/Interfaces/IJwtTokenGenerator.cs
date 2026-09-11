using IncidentManagement.Domain.Entities;

namespace IncidentManagement.Application.Common.Interfaces;

/// <summary>
/// Produces a signed JWT access token for an authenticated user.
/// Implemented by the infrastructure layer (holds the signing key and issuer settings).
/// </summary>
public interface IJwtTokenGenerator
{
    (string Token, DateTime ExpiresAtUtc) GenerateToken(User user);
}
