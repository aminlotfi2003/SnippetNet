namespace SnippetNet.Domain.Common.Abstractions;

public interface IEntity
{
    Guid Id { get; set; }
    DateTimeOffset CreatedAt { get; set; }
    DateTimeOffset? UpdatedAt { get; set; }
}
