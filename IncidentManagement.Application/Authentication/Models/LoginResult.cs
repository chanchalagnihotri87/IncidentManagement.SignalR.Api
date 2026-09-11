namespace IncidentManagement.Application.Authentication.Models;

/// <summary>Outcome of a login attempt. On success carries the JWT and its expiry.</summary>
public record LoginResult(bool Succeeded, string? AccessToken, DateTime? ExpiresAtUtc, string? Error)
{
    public static LoginResult Success(string accessToken, DateTime expiresAtUtc) =>
        new(true, accessToken, expiresAtUtc, null);

    public static LoginResult Failure(string error) =>
        new(false, null, null, error);
}
