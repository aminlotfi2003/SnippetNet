using SnippetNet.Application.Common.Abstractions.UoW;
using SnippetNet.Infrastructure.Persistence.Contexts;

namespace SnippetNet.Infrastructure.Persistence.UoW;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;

    public EfUnitOfWork(ApplicationDbContext db)
    {
        _db = db;
    }
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);

    public ValueTask DisposeAsync() => _db.DisposeAsync();
}
