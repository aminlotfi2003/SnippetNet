using MediatR;
using SnippetNet.Application.Common.Abstractions.Repositories;
using SnippetNet.Application.Common.Abstractions.UoW;
using SnippetNet.Application.Common.Exceptions;

namespace SnippetNet.Application.Snippets.Commands.UpdateSnippet;

public class UpdateSnippetCommandHandler : IRequestHandler<UpdateSnippetCommand, Unit>
{
    private readonly ISnippetRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateSnippetCommandHandler(ISnippetRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(UpdateSnippetCommand req, CancellationToken ct)
    {
        var result = await _repo.GetByIdAsync(req.Id, ct)
            ?? throw new NotFoundException("Snippet", req.Id);

        if (result.OwnerId != req.OwnerId)
            throw new NotFoundException("Snippet", req.Id);

        result.Title = req.Title.Trim();
        result.Description = req.Description?.Trim();
        result.Language = req.Language.Trim();
        result.TagName = req.TagName.Trim();
        result.Code = req.Code;

        _repo.Update(result);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
