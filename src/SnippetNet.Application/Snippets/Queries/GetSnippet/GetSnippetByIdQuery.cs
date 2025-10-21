using MediatR;
using SnippetNet.Application.Snippets.Dtos;

namespace SnippetNet.Application.Snippets.Queries.GetSnippet;

public record GetSnippetByIdQuery(Guid OwnerId, Guid Id) : IRequest<SnippetDto>;
