using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SnippetNet.Application.ViewModels;
using SnippetNet.Domain.Entities;

namespace SnippetNet.Application.Snippets.Queries.ListSnippets;

public class ListSnippetsHandler : IRequestHandler<ListSnippetsQuery, IReadOnlyList<SnippetListVm>>
{
    private readonly DbContext _db;
    private readonly IMapper _mapper;

    public ListSnippetsHandler(SnippetNet.Infrastructure.Persistence.AppDbContext db, IMapper mapper)
    {
        _db = db; _mapper = mapper;
    }

    public async Task<IReadOnlyList<SnippetListVm>> Handle(ListSnippetsQuery r, CancellationToken ct)
    {
        var query = _db.Set<Snippet>().AsNoTracking();

        if (!r.IsAdmin)
        {
            if (r.CurrentUserId is Guid uid)
                query = query.Where(s => s.IsPublic || s.OwnerId == uid);
            else
                query = query.Where(s => s.IsPublic);
        }

        if (!string.IsNullOrWhiteSpace(r.q))
        {
            var q = r.q.Trim();
            query = query.Where(s =>
                s.Title.Contains(q) ||
                (s.Description != null && s.Description.Contains(q)) ||
                s.Code.Contains(q));
        }

        if (!string.IsNullOrWhiteSpace(r.language))
            query = query.Where(s => s.Language == r.language);

        var skip = (r.page - 1) * r.pageSize;

        return await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip(skip).Take(r.pageSize)
            .ProjectTo<SnippetListVm>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);
    }
}
