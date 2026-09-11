using IncidentManagement.Domain.Common;
using IncidentManagement.Domain.Enums;

namespace IncidentManagement.Domain.Entities;

/// <summary>
/// An account that can authenticate against the API and be issued a JWT access token.
/// Password is stored as plain text for now to keep the SignalR practice setup simple;
/// swap <see cref="Password"/> for a hash before this goes anywhere real.
/// </summary>
public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;

    /// <summary>Unique login identifier. Compared case-insensitively by the application layer.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Plain-text password &mdash; temporary, replace with a hash later.</summary>
    public string Password { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Reporter;

    /// <summary>When false the user is blocked from logging in even with valid credentials.</summary>
    public bool IsActive { get; set; } = true;

    public DateTime? LastLoginAtUtc { get; set; }

    /// <summary>True when the account is allowed to complete a login and receive a token.</summary>
    public bool CanLogin => IsActive;

    public void RecordSuccessfulLogin(DateTime whenUtc)
    {
        LastLoginAtUtc = whenUtc;
        UpdatedAtUtc = whenUtc;
    }
}
