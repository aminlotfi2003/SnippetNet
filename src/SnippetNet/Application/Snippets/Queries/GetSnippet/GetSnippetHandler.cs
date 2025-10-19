using AutoMapper;
using MediatR;
using SnippetNet.Application.Abstractions;
using SnippetNet.Application.ViewModels;

namespace SnippetNet.Application.Snippets.Queries.GetSnippet;

public class GetSnippetHandler : IRequestHandler<GetSnippetQuery, SnippetDetailsVm>
{
    private readonly ISnippetRepository _snippets;
    private readonly IMapper _mapper;

    public GetSnippetHandler(ISnippetRepository snippets, IMapper mapper)
    {
        _snippets = snippets; _mapper = mapper;
    }

    public async Task<SnippetDetailsVm> Handle(GetSnippetQuery request, CancellationToken ct)
    {
        var entity = await _snippets.GetWithTagsAsync(request.Id, ct)
            ?? throw new KeyNotFoundException("Snippet not found.");

        if (!entity.IsPublic && !(request.IsAdmin || (request.CurrentUserId.HasValue && entity.OwnerId == request.CurrentUserId.Value)))
            throw new UnauthorizedAccessException("You cannot view this snippet.");

        return _mapper.Map<SnippetDetailsVm>(entity);
    }
}
