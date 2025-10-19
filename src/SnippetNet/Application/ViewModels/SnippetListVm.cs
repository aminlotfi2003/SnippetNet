namespace SnippetNet.Application.ViewModels;

public class SnippetListVm
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Language { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
