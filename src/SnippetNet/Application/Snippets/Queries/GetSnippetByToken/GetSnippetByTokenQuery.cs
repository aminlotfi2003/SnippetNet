using MediatR;
using SnippetNet.Application.ViewModels;

namespace SnippetNet.Application.Snippets.Queries.GetSnippetByToken;

public record GetSnippetByTokenQuery(string Token) : IRequest<SnippetDetailsVm>;
