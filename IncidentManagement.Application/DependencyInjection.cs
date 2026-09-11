using IncidentManagement.Application.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace IncidentManagement.Application;

/// <summary>
/// Registers the application layer's services (handlers, validators, etc.).
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}
