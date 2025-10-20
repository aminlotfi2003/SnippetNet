using MediatR;

namespace SnippetNet.Application.Snippets.Commands.DeleteSnippet;

public record DeleteSnippetCommand(Guid Id) : IRequest<Unit>;
