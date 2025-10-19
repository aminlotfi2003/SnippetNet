using Microsoft.EntityFrameworkCore;
using SnippetNet.Application.Abstractions;
using SnippetNet.Domain.Entities;

namespace SnippetNet.Infrastructure.Persistence.Repositories;

public class TagRepository : Repository<Tag>, ITagRepository
{
    public TagRepository(AppDbContext db) : base(db) { }

    public async Task<Tag> GetOrCreateAsync(string name, CancellationToken ct = default)
    {
        var existing = await _db.Tags.FirstOrDefaultAsync(t => t.Name == name, ct);
        if (existing != null) return existing;

        var tag = new Tag { Name = name };
        await _db.Tags.AddAsync(tag, ct);
        return tag;
    }
}
