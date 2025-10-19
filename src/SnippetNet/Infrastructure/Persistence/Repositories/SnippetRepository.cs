using Microsoft.EntityFrameworkCore;
using SnippetNet.Application.Abstractions;
using SnippetNet.Domain.Entities;

namespace SnippetNet.Infrastructure.Persistence.Repositories;

public class SnippetRepository : Repository<Snippet>, ISnippetRepository
{
    public SnippetRepository(AppDbContext db) : base(db) { }

    public Task<Snippet?> GetWithTagsAsync(Guid id, CancellationToken ct = default)
        => _db.Snippets
              .Include(s => s.SnippetTags).ThenInclude(st => st.Tag)
              .FirstOrDefaultAsync(s => s.Id == id, ct);

    public Task<bool> IsOwnerAsync(Guid snippetId, Guid ownerId, CancellationToken ct = default)
        => _db.Snippets.AnyAsync(s => s.Id == snippetId && s.OwnerId == ownerId, ct);
}
