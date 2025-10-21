using MediatR;
using SnippetNet.Application.Snippets.Dtos;

namespace SnippetNet.Application.Snippets.Queries.ListSnippets;

public record ListSnippetsQuery(Guid OwnerId) : IRequest<IReadOnlyList<SnippetDto>>;
