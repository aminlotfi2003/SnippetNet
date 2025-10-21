using MediatR;
using SnippetNet.Application.Common.Abstractions.Repositories;
using SnippetNet.Application.Common.Abstractions.UoW;
using SnippetNet.Application.Snippets.Dtos;
using SnippetNet.Domain.Entities;

namespace SnippetNet.Application.Snippets.Commands.CreateSnippet;

public class CreateSnippetCommandHandler : IRequestHandler<CreateSnippetCommand, SnippetDto>
{
    private readonly ISnippetRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateSnippetCommandHandler(
        ISnippetRepository repo,
        IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<SnippetDto> Handle(CreateSnippetCommand req, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(req);
        ct.ThrowIfCancellationRequested();

        var snippet = new Snippet
        {
            OwnerId = req.OwnerId,
            Title = req.Title,
            Description = req.Description,
            Language = req.Language,
            Code = req.Code,
            TagName = req.TagName
        };

        await _repo.AddAsync(snippet, ct);
        await _uow.SaveChangesAsync(ct);

        return SnippetDto.FromEntity(snippet);
    }
}
