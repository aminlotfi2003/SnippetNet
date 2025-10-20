using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Snippets.Commands.DeleteSnippet;
using SnippetNet.Application.Snippets.Dtos;
using SnippetNet.Application.Snippets.Queries.GetSnippet;

namespace SnippetNet.WebApp.Pages.Snippets;

public class DeleteModel : PageModel
{
    private readonly IMediator _mediator;
    public DeleteModel(IMediator mediator) => _mediator = mediator;

    public SnippetDto? Snippet { get; private set; }

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken ct)
    {
        Snippet = await _mediator.Send(new GetSnippetByIdQuery(id), ct);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteSnippetCommand(id), ct);
        TempData["SuccessMessage"] = "Snippet deleted.";
        return RedirectToPage("Index");
    }
}
