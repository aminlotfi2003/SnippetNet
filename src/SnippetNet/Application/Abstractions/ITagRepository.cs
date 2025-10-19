using SnippetNet.Domain.Entities;

namespace SnippetNet.Application.Abstractions;

public interface ITagRepository : IRepository<Tag>
{
    Task<Tag> GetOrCreateAsync(string name, CancellationToken ct = default);
}
