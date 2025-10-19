using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.DTOs.Snippets;
using SnippetNet.Application.Snippets.Commands.CreateSnippet;

namespace SnippetNet.Pages.Snippets;

[Authorize(Roles = "Contributor,Admin")]
public class CreateModel : PageModel
{
    private readonly IMediator _mediator;
    public CreateModel(IMediator mediator) => _mediator = mediator;

    [BindProperty] public CreateSnippetDto Input { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var (uid, isAdmin) = GetUser();
        if (uid == Guid.Empty) return Forbid();

        var id = await _mediator.Send(new CreateSnippetCommand(Input, uid));
        return RedirectToPage("./Details", new { id });
    }

    private (Guid, bool) GetUser()
    {
        var id = User.FindFirst("sub")?.Value ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        Guid.TryParse(id, out var guid);
        return (guid, User.IsInRole("Admin"));
    }
}
