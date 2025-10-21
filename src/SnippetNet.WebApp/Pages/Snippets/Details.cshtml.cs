using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Common.Exceptions;
using SnippetNet.Application.Snippets.Dtos;
using SnippetNet.Application.Snippets.Queries.GetSnippet;

namespace SnippetNet.WebApp.Pages.Snippets;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly IMediator _mediator;
    public DetailsModel(IMediator mediator) => _mediator = mediator;

    public SnippetDto? Snippet { get; set; }

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
}
