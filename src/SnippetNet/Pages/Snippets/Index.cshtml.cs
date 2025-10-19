using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Snippets.Queries.ListSnippets;
using SnippetNet.Application.ViewModels;

namespace SnippetNet.Pages.Snippets;

public class IndexModel : PageModel
{
    private readonly IMediator _mediator;
    public IndexModel(IMediator mediator) => _mediator = mediator;

    [BindProperty(SupportsGet = true)] public string? q { get; set; }
    [BindProperty(SupportsGet = true)] public string? language { get; set; }
    public IReadOnlyList<SnippetListVm> Items { get; set; } = [];

    public async Task OnGetAsync(int page = 1)
    {
        var (uid, isAdmin) = GetUser();
        Items = await _mediator.Send(new ListSnippetsQuery(q, language, page, 20, uid, isAdmin));
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
