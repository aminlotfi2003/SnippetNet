using SnippetNet.Domain.Common;

namespace SnippetNet.Domain.Entities;

public class Tag : EntityBase
{
    public string Name { get; set; } = default!;
    public ICollection<SnippetTag> SnippetTags { get; set; } = new List<SnippetTag>();
}
