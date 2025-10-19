using MediatR;

namespace SnippetNet.Application.Snippets.Commands.GenerateReadonlyLink;

public record GenerateReadonlyLinkCommand(Guid SnippetId, Guid CurrentUserId, bool IsAdmin) : IRequest<string>;
