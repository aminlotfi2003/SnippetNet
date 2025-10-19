namespace SnippetNet.Domain.Entities;

public class SnippetTag
{
    public Guid SnippetId { get; set; }
    public Guid TagId { get; set; }

    public Snippet Snippet { get; set; } = default!;
    public Tag Tag { get; set; } = default!;
}
