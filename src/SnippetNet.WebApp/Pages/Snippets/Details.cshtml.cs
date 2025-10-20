using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Snippets.Dtos;
using SnippetNet.Application.Snippets.Queries.GetSnippet;

namespace SnippetNet.WebApp.Pages.Snippets;

public class DetailsModel : PageModel
{
    private readonly IMediator _mediator;
    public DetailsModel(IMediator mediator) => _mediator = mediator;

    public SnippetDto? Snippet { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken ct)
    {
        Snippet = await _mediator.Send(new GetSnippetByIdQuery(id), ct);
        return Page();
    }
}
