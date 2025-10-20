using SnippetNet.Domain.Entities;
using SnippetNet.Domain.Enums;

namespace SnippetNet.Application.Snippets.Dtos;

public sealed record SnippetDto(
    Guid Id,
    string Title,
    string? Description,
    string Language,
    string Code,
    string TagName)
{
    public static SnippetDto FromEntity(Snippet snippet) =>
        new(
            snippet.Id,
            snippet.Title,
            snippet.Description!,
            snippet.Language,
            snippet.Code,
            snippet.TagName
        );
}
