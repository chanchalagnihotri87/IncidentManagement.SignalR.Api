using IncidentManagement.Application.Common.Interfaces;
using IncidentManagement.Domain.Entities;
using IncidentManagement.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace IncidentManagement.Infrastructure.Persistence.Repositories;

/// <summary>EF Core-backed <see cref="IServiceRepository"/> against IncidentManagementDB.</summary>
public class ServiceRepository : BaseRepository<Service, Guid>, IServiceRepository
{
    public ServiceRepository(IncidentManagementDbContext dbContext) : base(dbContext)
    {
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default) =>
        DbSet.AnyAsync(s => s.Name.ToLower() == name.ToLower(), cancellationToken);
}
