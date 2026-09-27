using IncidentManagement.Application.Authentication;
using IncidentManagement.Application.Incidents;
using IncidentManagement.Application.Services;
using IncidentManagement.Application.Teams;
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
        services.AddScoped<IIncidentService, IncidentService>();
        services.AddScoped<IServiceCatalogService, ServiceCatalogService>();
        services.AddScoped<ITeamManagementService, TeamManagementService>();
        services.AddScoped<ITeamMemberService, TeamMemberService>();
        return services;
    }
}
