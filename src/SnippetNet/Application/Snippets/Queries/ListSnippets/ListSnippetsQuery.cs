using MediatR;
using SnippetNet.Application.ViewModels;

namespace SnippetNet.Application.Snippets.Queries.ListSnippets;

public record ListSnippetsQuery(string? q, string? language, int page = 1, int pageSize = 20,
                                Guid? CurrentUserId = null, bool IsAdmin = false) : IRequest<IReadOnlyList<SnippetListVm>>;
