using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SnippetNet.Application.ViewModels;
using SnippetNet.Domain.Entities;
using SnippetNet.Infrastructure.Persistence;

namespace SnippetNet.Application.Snippets.Queries.GetSnippetByToken;

public class GetSnippetByTokenHandler : IRequestHandler<GetSnippetByTokenQuery, SnippetDetailsVm>
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    public GetSnippetByTokenHandler(AppDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<SnippetDetailsVm> Handle(GetSnippetByTokenQuery request, CancellationToken ct)
    {
        var entity = await _db.Set<Snippet>()
            .Include(s => s.SnippetTags).ThenInclude(st => st.Tag)
            .FirstOrDefaultAsync(s => s.ReadOnlyToken == request.Token, ct)
            ?? throw new KeyNotFoundException("Invalid or expired link.");

        return _mapper.Map<SnippetDetailsVm>(entity);
    }
}
