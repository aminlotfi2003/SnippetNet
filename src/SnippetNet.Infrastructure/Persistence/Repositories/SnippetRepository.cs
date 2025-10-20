using SnippetNet.Application.Common.Abstractions.Repositories;
using SnippetNet.Domain.Entities;
using SnippetNet.Infrastructure.Persistence.Contexts;

namespace SnippetNet.Infrastructure.Persistence.Repositories;

public class SnippetRepository : EfRepository<Snippet>, ISnippetRepository
{
    public SnippetRepository(ApplicationDbContext db) : base(db) { }
}
