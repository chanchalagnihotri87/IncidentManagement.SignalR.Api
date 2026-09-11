using IncidentManagement.Application.Common.Interfaces;
using IncidentManagement.Infrastructure.Authentication;
using IncidentManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IncidentManagement.Infrastructure;

/// <summary>
/// Registers the infrastructure layer's services (persistence, external integrations).
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("IncidentManagementDb");

        services.AddDbContext<IncidentManagementDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IUserRepository, UserRepository>();

        var jwtSettings = new JwtSettings
        {
            Issuer = configuration[$"{JwtSettings.SectionName}:Issuer"]
                ?? throw new InvalidOperationException("Missing 'Jwt:Issuer' configuration."),
            Audience = configuration[$"{JwtSettings.SectionName}:Audience"]
                ?? throw new InvalidOperationException("Missing 'Jwt:Audience' configuration."),
            SecretKey = configuration[$"{JwtSettings.SectionName}:SecretKey"]
                ?? throw new InvalidOperationException("Missing 'Jwt:SecretKey' configuration."),
            ExpiryMinutes = int.TryParse(configuration[$"{JwtSettings.SectionName}:ExpiryMinutes"], out var minutes)
                ? minutes
                : 60
        };

        services.AddSingleton(jwtSettings);
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
