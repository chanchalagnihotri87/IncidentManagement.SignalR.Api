namespace IncidentManagement.Domain.Enums;

/// <summary>
/// Application roles used for authorization once a user is authenticated via JWT.
/// The value is emitted as a role claim in the access token.
/// </summary>
public enum UserRole
{
    /// <summary>Can raise and view their own incidents.</summary>
    Reporter,

    /// <summary>Can triage, update and resolve incidents.</summary>
    Agent,

    /// <summary>Full access, including user administration.</summary>
    Administrator
}
