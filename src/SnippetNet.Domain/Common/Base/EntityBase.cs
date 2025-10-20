using SnippetNet.Domain.Common.Abstractions;

namespace SnippetNet.Domain.Common.Base;

public abstract class EntityBase : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }

    public void Touch() => UpdatedAt = DateTimeOffset.UtcNow;
}
