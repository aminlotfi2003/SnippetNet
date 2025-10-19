using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Snippets.Commands.DeleteSnippet;
using SnippetNet.Application.Snippets.Queries.GetSnippet;

namespace SnippetNet.Pages.Snippets;

[Authorize(Roles = "Contributor,Admin")]
public class DeleteModel : PageModel
{
    private readonly IMediator _mediator;
    public DeleteModel(IMediator mediator) => _mediator = mediator;

    public SnippetNet.Application.ViewModels.SnippetDetailsVm Vm { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var (uid, isAdmin) = GetUser();
        Vm = await _mediator.Send(new GetSnippetQuery(id, uid, isAdmin));
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        var (uid, isAdmin) = GetUser();
        await _mediator.Send(new DeleteSnippetCommand(id, uid, isAdmin));
        TempData["StatusMessage"] = "Snippet deleted.";
        return RedirectToPage("./Index");
    }

    private (Guid, bool) GetUser()
    {
        var id = User.FindFirst("sub")?.Value
                 ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        Guid.TryParse(id, out var guid);
        return (guid, User.IsInRole("Admin"));
    }
}
