using AutoMapper;
using MediatR;
using SnippetNet.Application.Abstractions;
using SnippetNet.Domain.Entities;

namespace SnippetNet.Application.Snippets.Commands.CreateSnippet;

public class CreateSnippetHandler : IRequestHandler<CreateSnippetCommand, Guid>
{
    private readonly ISnippetRepository _snippets;
    private readonly ITagRepository _tags;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateSnippetHandler(ISnippetRepository snippets, ITagRepository tags, IUnitOfWork uow, IMapper mapper)
    {
        _snippets = snippets; _tags = tags; _uow = uow; _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateSnippetCommand request, CancellationToken ct)
    {
        var entity = _mapper.Map<Snippet>(request.Dto);
        entity.OwnerId = request.CurrentUserId;

        var uniqueTags = request.Dto.Tags.Select(t => t.Trim())
                                         .Where(t => !string.IsNullOrWhiteSpace(t))
                                         .Distinct(StringComparer.OrdinalIgnoreCase)
                                         .ToList();

        var snippetTags = new List<SnippetTag>();
        foreach (var name in uniqueTags)
        {
            var tag = await _tags.GetOrCreateAsync(name, ct);
            snippetTags.Add(new SnippetTag { Snippet = entity, Tag = tag });
        }
        entity.SnippetTags = snippetTags;

        await _snippets.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);

        return entity.Id;
    }
}
