using MediatR;
using SnippetNet.Application.Snippets.Dtos;

namespace SnippetNet.Application.Snippets.Commands.CreateSnippet;

public record CreateSnippetCommand(
    Guid OwnerId,
    string Title,
    string? Description,
    string Language,
    string Code,
    string TagName
) : IRequest<SnippetDto>;
