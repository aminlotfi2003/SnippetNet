using SnippetNet.Domain.Common.Base;
using SnippetNet.Domain.Enums;

namespace SnippetNet.Domain.Entities;

public class Snippet : EntityBase
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public Language Language { get; set; }
    public string Code { get; set; } = default!;
    public string TagName { get; set; } = default!;
}
