namespace IncidentManagement.Application.Authentication.Models;

/// <summary>Credentials supplied by a client to obtain a JWT access token.</summary>
public record LoginRequest(string Email, string Password);
