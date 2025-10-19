using MediatR;
using SnippetNet.Application.ViewModels;

namespace SnippetNet.Application.Snippets.Queries.GetSnippet;

public record GetSnippetQuery(Guid Id, Guid? CurrentUserId, bool IsAdmin) : IRequest<SnippetDetailsVm>;
