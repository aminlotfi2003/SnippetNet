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

    public IReadOnlyList<SnippetDto> Snippets { get; set; } = [];

    public async Task OnGetAsync(CancellationToken ct)
    {
        Snippets = await _mediator.Send(new ListSnippetsQuery(), ct);
    }

    public string? SuccessMessage
    {
        get => TempData[nameof(SuccessMessage)] as string;
        set => TempData[nameof(SuccessMessage)] = value;
    }
}
