using MediatR;

namespace SnippetNet.Application.Snippets.Commands.DeleteSnippet;

public record DeleteSnippetCommand(Guid OwnerId, Guid Id) : IRequest<Unit>;
