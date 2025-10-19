using AutoMapper;
using MediatR;
using SnippetNet.Application.Abstractions;
using SnippetNet.Domain.Entities;

namespace SnippetNet.Application.Snippets.Commands.UpdateSnippet;

public class UpdateSnippetHandler : IRequestHandler<UpdateSnippetCommand, Unit>
{
    private readonly ISnippetRepository _snippets;
    private readonly ITagRepository _tags;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdateSnippetHandler(ISnippetRepository snippets, ITagRepository tags, IUnitOfWork uow, IMapper mapper)
    {
        _snippets = snippets; _tags = tags; _uow = uow; _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateSnippetCommand request, CancellationToken ct)
    {
        var existing = await _snippets.GetWithTagsAsync(request.Dto.Id, ct)
            ?? throw new KeyNotFoundException("Snippet not found.");

        if (!request.IsAdmin && existing.OwnerId != request.CurrentUserId)
            throw new UnauthorizedAccessException("You cannot edit this snippet.");

        existing.Title = request.Dto.Title;
        existing.Language = request.Dto.Language;
        existing.Description = request.Dto.Description;
        existing.Code = request.Dto.Code;
        existing.IsPublic = request.Dto.IsPublic;

        var currentTags = existing.SnippetTags.ToList();
        existing.SnippetTags.Clear();

        var uniqueTags = request.Dto.Tags.Select(t => t.Trim())
                                         .Where(t => !string.IsNullOrWhiteSpace(t))
                                         .Distinct(StringComparer.OrdinalIgnoreCase);

        foreach (var name in uniqueTags)
        {
            var tag = await _tags.GetOrCreateAsync(name, ct);
            existing.SnippetTags.Add(new SnippetTag { SnippetId = existing.Id, TagId = tag.Id });
        }


        _snippets.Update(existing);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
