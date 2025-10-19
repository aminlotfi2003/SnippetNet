using MediatR;

namespace SnippetNet.Application.Snippets.Commands.DeleteSnippet;

public record DeleteSnippetCommand(Guid Id, Guid CurrentUserId, bool IsAdmin) : IRequest<Unit>;
