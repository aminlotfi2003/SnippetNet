using MediatR;
using SnippetNet.Application.Common.Abstractions.Repositories;
using SnippetNet.Application.Snippets.Dtos;

namespace SnippetNet.Application.Snippets.Queries.ListSnippets;

public class ListSnippetsQueryHandler : IRequestHandler<ListSnippetsQuery, IReadOnlyList<SnippetDto>>
{
    private readonly ISnippetRepository _repo;

    public ListSnippetsQueryHandler(ISnippetRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyList<SnippetDto>> Handle(ListSnippetsQuery req, CancellationToken ct)
    {
        var result = await _repo.ListAsync(null, ct);

        return result.Select(SnippetDto.FromEntity).ToList();
    }
}
