using MediatR;
using SnippetNet.Application.Common.Abstractions.Repositories;
using SnippetNet.Application.Common.Exceptions;
using SnippetNet.Application.Snippets.Dtos;

namespace SnippetNet.Application.Snippets.Queries.GetSnippet;

public class GetSnippetByIdQueryHandler : IRequestHandler<GetSnippetByIdQuery, SnippetDto>
{
    private readonly ISnippetRepository _repo;

    public GetSnippetByIdQueryHandler(ISnippetRepository repo)
    {
        _repo = repo;
    }

    public async Task<SnippetDto> Handle(GetSnippetByIdQuery req, CancellationToken ct)
    {
        var result = await _repo.GetByIdAsync(req.Id, ct)
            ?? throw new NotFoundException("Snippet", req.Id);

        return SnippetDto.FromEntity(result);
    }
}
