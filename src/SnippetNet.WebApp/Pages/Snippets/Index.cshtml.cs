using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Snippets.Dtos;
using SnippetNet.Application.Snippets.Queries.ListSnippets;

namespace SnippetNet.WebApp.Pages.Snippets;

public class IndexModel : PageModel
{
    private readonly IMediator _mediator;

    public IndexModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    public IReadOnlyList<SnippetDto> Snippets { get; private set; } = [];

    public bool RequiresAuthentication => !User.Identity?.IsAuthenticated ?? true;

    public async Task OnGetAsync(CancellationToken ct)
    {
        if (RequiresAuthentication)
        {
            Snippets = [];
            return;
        }

        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        Snippets = await _mediator.Send(new ListSnippetsQuery(userId), ct);
    }

    public string? SuccessMessage
    {
        get => TempData[nameof(SuccessMessage)] as string;
        set => TempData[nameof(SuccessMessage)] = value;
    }
}
