using IncidentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IncidentManagement.Infrastructure.Persistence.Context;

/// <summary>
/// EF Core context for IncidentManagementDB (SQL Server).
/// </summary>
public class IncidentManagementDbContext : DbContext
{
    public IncidentManagementDbContext(DbContextOptions<IncidentManagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Incident> Incidents => Set<Incident>();

    public DbSet<Service> Services => Set<Service>();

    public DbSet<Team> Teams => Set<Team>();

    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IncidentManagementDbContext).Assembly);
    }
}
