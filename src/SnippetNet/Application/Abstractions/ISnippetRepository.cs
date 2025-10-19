using SnippetNet.Domain.Entities;

namespace SnippetNet.Application.Abstractions;

public interface ISnippetRepository : IRepository<Snippet>
{
    Task<Snippet?> GetWithTagsAsync(Guid id, CancellationToken ct = default);
    Task<bool> IsOwnerAsync(Guid snippetId, Guid ownerId, CancellationToken ct = default);
}
