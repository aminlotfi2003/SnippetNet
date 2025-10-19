using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Snippets.Queries.GetSnippet;

namespace SnippetNet.Pages.Snippets;

public class DetailsModel : PageModel
{
    private readonly IMediator _mediator;
    public DetailsModel(IMediator mediator) => _mediator = mediator;

    public SnippetNet.Application.ViewModels.SnippetDetailsVm Vm { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var (uid, isAdmin) = GetUser();
        Vm = await _mediator.Send(new GetSnippetQuery(id, uid, isAdmin));
        return Page();
    }

    private (Guid?, bool) GetUser()
    {
        if (User?.Identity?.IsAuthenticated == true)
        {
            var id = User.FindFirst("sub")?.Value ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            Guid.TryParse(id, out var guid);
            var isAdmin = User.IsInRole("Admin");
            return (guid, isAdmin);
        }
        return (null, false);
    }
}
