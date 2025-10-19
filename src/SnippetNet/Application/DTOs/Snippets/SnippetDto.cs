namespace SnippetNet.Application.DTOs.Snippets;

public class SnippetDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Language { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public List<string> Tags { get; set; } = new();
    public DateTimeOffset CreatedAt { get; set; }
}
