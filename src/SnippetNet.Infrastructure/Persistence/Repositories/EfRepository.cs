using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SnippetNet.Application.Common.Abstractions.Repositories;
using SnippetNet.Domain.Common.Base;
using SnippetNet.Infrastructure.Persistence.Contexts;

namespace SnippetNet.Infrastructure.Persistence.Repositories;

public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
{
    protected readonly ApplicationDbContext _db;
    protected DbSet<TEntity> Set => _db.Set<TEntity>();

    public EfRepository(ApplicationDbContext db) => _db = db;

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await Set.FindAsync([id], ct);

    public virtual async Task<IReadOnlyList<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default)
        => predicate is null ? await Set.AsNoTracking().ToListAsync(ct)
                             : await Set.AsNoTracking().Where(predicate).ToListAsync(ct);

    public virtual Task AddAsync(TEntity entity, CancellationToken ct = default)
        => Set.AddAsync(entity, ct).AsTask();

    public virtual void Update(TEntity entity) => Set.Update(entity);
    public virtual void Remove(TEntity entity) => Set.Remove(entity);
}
