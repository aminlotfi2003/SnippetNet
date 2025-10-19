using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.DTOs.Snippets;
using SnippetNet.Application.Snippets.Commands.UpdateSnippet;
using SnippetNet.Application.Snippets.Queries.GetSnippet;

namespace SnippetNet.Pages.Snippets;

[Authorize(Roles = "Contributor,Admin")]
public class EditModel : PageModel
{
    private readonly IMediator _mediator;
    public EditModel(IMediator mediator) => _mediator = mediator;

    [BindProperty] public UpdateSnippetDto Input { get; set; } = new();
    [BindProperty] public string TagsCsv { get; set; } = string.Empty;

    [TempData] public string? StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var (uid, isAdmin) = GetUser();
        var vm = await _mediator.Send(new GetSnippetQuery(id, uid, isAdmin));
        Input = new UpdateSnippetDto
        {
            Id = vm.Id,
            Title = vm.Title,
            Language = vm.Language,
            Description = vm.Description,
            Code = vm.Code,
            IsPublic = vm.IsPublic,
            Tags = vm.Tags
        };
        TagsCsv = string.Join(",", vm.Tags);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Input.Tags = (TagsCsv ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (!ModelState.IsValid) return Page();

        var (uid, isAdmin) = GetUser();
        await _mediator.Send(new UpdateSnippetCommand(Input, uid, isAdmin));

        StatusMessage = "Snippet updated successfully.";
        return RedirectToPage("./Details", new { id = Input.Id });
    }

    private (Guid, bool) GetUser()
    {
        var id = User.FindFirst("sub")?.Value
                 ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        Guid.TryParse(id, out var guid);
        return (guid, User.IsInRole("Admin"));
    }
}
