using MediatR;
using SnippetNet.Application.Common.Abstractions.Repositories;
using SnippetNet.Application.Common.Abstractions.UoW;

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
            ?? throw new KeyNotFoundException("Snippet not found.");

        _repo.Remove(result);
        await _uow.SaveChangesAsync();
        return Unit.Value;
    }
}
