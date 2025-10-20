using MediatR;
using SnippetNet.Application.Snippets.Dtos;
using SnippetNet.Domain.Enums;

namespace SnippetNet.Application.Snippets.Commands.CreateSnippet;

public record CreateSnippetCommand(
    string Title,
    string? Description,
    Language Language,
    string Code,
    string TagName
) : IRequest<SnippetDto>;
