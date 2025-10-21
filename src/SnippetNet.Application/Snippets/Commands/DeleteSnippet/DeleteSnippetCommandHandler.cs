using MediatR;
using SnippetNet.Application.Common.Abstractions.Repositories;
using SnippetNet.Application.Common.Abstractions.UoW;
using SnippetNet.Application.Common.Exceptions;

namespace SnippetNet.Application.Snippets.Commands.DeleteSnippet;

public class DeleteSnippetCommandHandler : IRequestHandler<DeleteSnippetCommand, Unit>
{
    private readonly ISnippetRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteSnippetCommandHandler(ISnippetRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteSnippetCommand req, CancellationToken ct)
    {
        var result = await _repo.GetByIdAsync(req.Id, ct)
            ?? throw new NotFoundException("Snippet", req.Id);

        if (result.OwnerId != req.OwnerId)
            throw new NotFoundException("Snippet", req.Id);

        _repo.Remove(result);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
