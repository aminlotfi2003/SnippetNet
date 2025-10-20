using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Snippets.Commands.UpdateSnippet;
using SnippetNet.Application.Snippets.Queries.GetSnippet;

namespace SnippetNet.WebApp.Pages.Snippets;

public class EditModel : PageModel
{
    private readonly IMediator _mediator;

    public EditModel(IMediator mediator) => _mediator = mediator;

    [BindProperty] public UpdateSnippetCommand Command { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken ct)
    {
        var x = await _mediator.Send(new GetSnippetByIdQuery(id), ct);

        Command = new UpdateSnippetCommand(
            x.Id, x.Title, x.Description, x.Language, x.Code, x.TagName);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid) return Page();

        await _mediator.Send(Command, ct);
        TempData["SuccessMessage"] = "Snippet updated.";
        return RedirectToPage("Index");
    }
}
