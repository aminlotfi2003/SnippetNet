using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Common.Exceptions;
using SnippetNet.Application.Snippets.Commands.DeleteSnippet;
using SnippetNet.Application.Snippets.Dtos;
using SnippetNet.Application.Snippets.Queries.GetSnippet;

namespace SnippetNet.WebApp.Pages.Snippets;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly IMediator _mediator;
    public DeleteModel(IMediator mediator) => _mediator = mediator;

    public SnippetDto? Snippet { get; private set; }

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken ct)
    {
        try
        {
            var ownerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            Snippet = await _mediator.Send(new GetSnippetByIdQuery(ownerId, id), ct);
            return Page();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    public async Task<IActionResult> OnPostAsync(Guid id, CancellationToken ct)
    {
        try
        {
            var ownerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _mediator.Send(new DeleteSnippetCommand(ownerId, id), ct);
            TempData["SuccessMessage"] = "Snippet deleted.";
            return RedirectToPage("Index");
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }
}
