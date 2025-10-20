using MediatR;
using SnippetNet.Application.Snippets.Dtos;
using SnippetNet.Domain.Enums;

namespace SnippetNet.Application.Snippets.Commands.UpdateSnippet;

public record UpdateSnippetCommand(
    Guid Id,
    string Title,
    string? Description,
    Language Language,
    string Code,
    string TagName
) : IRequest<SnippetDto>;
