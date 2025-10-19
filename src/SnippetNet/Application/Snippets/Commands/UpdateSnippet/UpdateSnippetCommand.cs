using MediatR;
using SnippetNet.Application.DTOs.Snippets;

namespace SnippetNet.Application.Snippets.Commands.UpdateSnippet;

public record UpdateSnippetCommand(UpdateSnippetDto Dto, Guid CurrentUserId, bool IsAdmin) : IRequest<Unit>;
