using IncidentManagement.Application.Authentication.Models;
using IncidentManagement.Application.Common.Interfaces;

namespace IncidentManagement.Application.Authentication;

/// <summary>
/// Validates credentials against the user store and, on success, returns a signed JWT.
/// Password check is a plain-text comparison for now &mdash; swap for a hash verifier later.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        // Same message whether the e-mail is unknown or the password is wrong.
        if (user is null || !string.Equals(user.Password, request.Password, StringComparison.Ordinal))
        {
            return LoginResult.Failure("Invalid e-mail or password.");
        }

        if (!user.CanLogin)
        {
            return LoginResult.Failure("This account is disabled.");
        }

        var (token, expiresAtUtc) = _jwtTokenGenerator.GenerateToken(user);

        user.RecordSuccessfulLogin(DateTime.UtcNow);
        await _userRepository.UpdateAsync(user, cancellationToken);

        return LoginResult.Success(token, expiresAtUtc);
    }
}
