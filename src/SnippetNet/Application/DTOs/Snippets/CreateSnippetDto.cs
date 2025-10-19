namespace SnippetNet.Application.DTOs.Snippets;

public class CreateSnippetDto
{
    public string Title { get; set; } = default!;
    public string Language { get; set; } = "PlainText";
    public string? Description { get; set; }
    public string Code { get; set; } = default!;
    public bool IsPublic { get; set; }
    public List<string> Tags { get; set; } = new();
}
