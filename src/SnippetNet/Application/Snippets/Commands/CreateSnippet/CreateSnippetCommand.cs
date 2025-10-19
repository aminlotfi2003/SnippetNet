using MediatR;
using SnippetNet.Application.DTOs.Snippets;

namespace SnippetNet.Application.Snippets.Commands.CreateSnippet;

public record CreateSnippetCommand(CreateSnippetDto Dto, Guid CurrentUserId) : IRequest<Guid>;
