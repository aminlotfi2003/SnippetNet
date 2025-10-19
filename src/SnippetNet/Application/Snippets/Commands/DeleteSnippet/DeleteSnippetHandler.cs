using MediatR;
using SnippetNet.Application.Abstractions;

namespace SnippetNet.Application.Snippets.Commands.DeleteSnippet;

public class DeleteSnippetHandler : IRequestHandler<DeleteSnippetCommand, Unit>
{
    private readonly ISnippetRepository _snippets;
    private readonly IUnitOfWork _uow;

    public DeleteSnippetHandler(ISnippetRepository snippets, IUnitOfWork uow)
    {
        _snippets = snippets; _uow = uow;
    }

    public async Task<Unit> Handle(DeleteSnippetCommand request, CancellationToken ct)
    {
        var entity = await _snippets.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException("Snippet not found.");

        if (!request.IsAdmin)
        {
            var isOwner = await _snippets.IsOwnerAsync(request.Id, request.CurrentUserId, ct);
            if (!isOwner) throw new UnauthorizedAccessException("You cannot delete this snippet.");
        }

        _snippets.Remove(entity);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
