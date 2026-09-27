using System.Linq.Expressions;
using System.Reflection;
using IncidentManagement.Application.Common.Interfaces;
using IncidentManagement.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace IncidentManagement.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core-backed base repository providing the CRUD operations shared by every entity.
/// Not every entity derives from <c>BaseEntity</c> (e.g. <c>Incident</c> manages its own "Id"),
/// and audit-timestamp properties are named differently across entities ("UpdatedAtUtc" vs
/// "UpdatedDate"), so reflection is used once per closed generic type to resolve those members
/// instead of duplicating this plumbing in every repository.
/// </summary>
public abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
    where TEntity : class
{
    private static readonly string[] UpdatedTimestampCandidates = { "UpdatedAtUtc", "UpdatedDate", "UpdatedAt" };

    private static readonly PropertyInfo IdProperty = ResolveIdProperty();
    private static readonly PropertyInfo? UpdatedTimestampProperty = ResolveUpdatedTimestampProperty();

    protected readonly IncidentManagementDbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;

    protected BaseRepository(IncidentManagementDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<TEntity>();
    }

    public virtual Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default) =>
        DbSet.FirstOrDefaultAsync(BuildIdEquals(id), cancellationToken);

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await DbSet.AsNoTracking().ToListAsync(cancellationToken);

    public virtual Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default) =>
        DbSet.AnyAsync(BuildIdEquals(id), cancellationToken);

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        UpdatedTimestampProperty?.SetValue(entity, DateTime.UtcNow);
        DbSet.Update(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>Builds "e =&gt; e.Id == id" for whatever the entity's "Id" property turns out to be.</summary>
    private static Expression<Func<TEntity, bool>> BuildIdEquals(TKey id)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "e");
        var property = Expression.Property(parameter, IdProperty);
        var value = Expression.Constant(id, IdProperty.PropertyType);
        return Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(property, value), parameter);
    }

    private static PropertyInfo ResolveIdProperty()
    {
        var property = typeof(TEntity).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance)
            ?? throw new InvalidOperationException($"{typeof(TEntity).Name} does not expose a public 'Id' property.");

        if (property.PropertyType != typeof(TKey))
        {
            throw new InvalidOperationException(
                $"{typeof(TEntity).Name}.Id is of type {property.PropertyType.Name}, but repository key type is {typeof(TKey).Name}.");
        }

        return property;
    }

    private static PropertyInfo? ResolveUpdatedTimestampProperty()
    {
        var type = typeof(TEntity);
        foreach (var name in UpdatedTimestampCandidates)
        {
            var property = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            if (property is not null
                && property.CanWrite
                && (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?)))
            {
                return property;
            }
        }

        return null;
    }
}
