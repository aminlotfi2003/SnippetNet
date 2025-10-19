using MediatR;
using SnippetNet.Application.Abstractions;

namespace SnippetNet.Application.Snippets.Commands.GenerateReadonlyLink;

public class GenerateReadonlyLinkHandler : IRequestHandler<GenerateReadonlyLinkCommand, string>
{
    private readonly ISnippetRepository _snippets;
    private readonly IUnitOfWork _uow;

    public GenerateReadonlyLinkHandler(ISnippetRepository snippets, IUnitOfWork uow)
    {
        _snippets = snippets; _uow = uow;
    }

    public async Task<string> Handle(GenerateReadonlyLinkCommand request, CancellationToken ct)
    {
        var entity = await _snippets.GetByIdAsync(request.SnippetId, ct)
            ?? throw new KeyNotFoundException("Snippet not found.");

        if (!request.IsAdmin && entity.OwnerId != request.CurrentUserId)
            throw new UnauthorizedAccessException("You cannot share this snippet.");

        entity.ReadOnlyToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                                 .Replace("+", "").Replace("/", "").Replace("=", "");
        _snippets.Update(entity);
        await _uow.SaveChangesAsync(ct);
        return entity.ReadOnlyToken!;
    }
}
