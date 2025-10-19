using Microsoft.EntityFrameworkCore;
using SnippetNet.Application.Abstractions;

namespace SnippetNet.Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _db;
    protected DbSet<T> Set => _db.Set<T>();

    public Repository(AppDbContext db) => _db = db;

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await Set.FindAsync([id], ct);

    public virtual async Task<IReadOnlyList<T>> ListAsync(
        System.Linq.Expressions.Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default)
        => predicate is null ? await Set.AsNoTracking().ToListAsync(ct)
                             : await Set.AsNoTracking().Where(predicate).ToListAsync(ct);

    public virtual Task AddAsync(T entity, CancellationToken ct = default)
        => Set.AddAsync(entity, ct).AsTask();

    public virtual void Update(T entity) => Set.Update(entity);
    public virtual void Remove(T entity) => Set.Remove(entity);
}
