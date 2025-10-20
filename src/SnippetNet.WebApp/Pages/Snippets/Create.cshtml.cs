using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Common.Exceptions;
using SnippetNet.Application.Snippets.Commands.CreateSnippet;

namespace SnippetNet.WebApp.Pages.Snippets;

public class CreateModel : PageModel
{
    private readonly IMediator _mediator;

    public CreateModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    [BindProperty] public CreateSnippetCommand Command { get; set; } = default!;

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var id = await _mediator.Send(Command, ct);

            TempData["SuccessMessage"] = "Product created successfully.";
            return RedirectToPage("Index");
        }
        catch (ConflictException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Unexpected error: {ex.Message}");
            return Page();
        }
    }
}
