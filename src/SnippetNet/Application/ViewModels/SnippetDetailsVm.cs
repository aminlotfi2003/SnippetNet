namespace SnippetNet.Application.ViewModels;

public class SnippetDetailsVm
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Language { get; set; } = default!;
    public string? Description { get; set; }
    public string Code { get; set; } = default!;
    public bool IsPublic { get; set; }
    public List<string> Tags { get; set; } = new();
    public string? ReadOnlyToken { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
