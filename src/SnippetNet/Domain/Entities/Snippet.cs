using SnippetNet.Domain.Common;

namespace SnippetNet.Domain.Entities;

public class Snippet : EntityBase
{
    public string Title { get; set; } = default!;
    public string Language { get; set; } = "PlainText";
    public string? Description { get; set; }
    public string Code { get; set; } = default!;
    public bool IsPublic { get; set; }
    public string? ReadOnlyToken { get; set; }

    public Guid OwnerId { get; set; }

    public ICollection<SnippetTag> SnippetTags { get; set; } = new List<SnippetTag>();
}
