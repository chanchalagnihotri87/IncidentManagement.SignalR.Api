namespace IncidentManagement.Infrastructure.Authentication;

/// <summary>Binds the "Jwt" section of configuration. Used to sign and stamp access tokens.</summary>
public class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; } = string.Empty;

    public string Audience { get; init; } = string.Empty;

    /// <summary>Symmetric signing key. Keep it out of source control in a real deployment.</summary>
    public string SecretKey { get; init; } = string.Empty;

    public int ExpiryMinutes { get; init; } = 60;
}
