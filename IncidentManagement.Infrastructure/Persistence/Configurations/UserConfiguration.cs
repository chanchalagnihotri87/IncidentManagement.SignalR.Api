using IncidentManagement.Domain.Entities;
using IncidentManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IncidentManagement.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.IsActive)
            .IsRequired();

        var seededAtUtc = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            new User
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                FullName = "Alice Admin",
                Email = "admin@incident.local",
                Password = "Admin@123",
                Role = UserRole.Administrator,
                IsActive = true,
                CreatedAtUtc = seededAtUtc
            },
            new User
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                FullName = "Ethan Agent",
                Email = "agent@incident.local",
                Password = "Agent@123",
                Role = UserRole.Agent,
                IsActive = true,
                CreatedAtUtc = seededAtUtc
            },
            new User
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                FullName = "Rachel Reporter",
                Email = "reporter@incident.local",
                Password = "Reporter@123",
                Role = UserRole.Reporter,
                IsActive = true,
                CreatedAtUtc = seededAtUtc
            });
    }
}
