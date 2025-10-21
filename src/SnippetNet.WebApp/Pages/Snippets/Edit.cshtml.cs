using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Common.Exceptions;
using SnippetNet.Application.Snippets.Commands.UpdateSnippet;
using SnippetNet.Application.Snippets.Queries.GetSnippet;

namespace SnippetNet.WebApp.Pages.Snippets;

[Authorize]
public class EditModel : PageModel
{
    private readonly IMediator _mediator;

    public EditModel(IMediator mediator) => _mediator = mediator;

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken ct)
    {
        try
        {
            var ownerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var snippet = await _mediator.Send(new GetSnippetByIdQuery(ownerId, id), ct);

            Input = new InputModel
            {
                Id = snippet.Id,
                Title = snippet.Title,
                Description = snippet.Description,
                Language = snippet.Language,
                Code = snippet.Code,
                TagName = snippet.TagName
            };

            return Page();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            var ownerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var command = new UpdateSnippetCommand(
                ownerId,
                Input.Id,
                Input.Title!,
                Input.Description,
                Input.Language!,
                Input.Code!,
                Input.TagName!);

            await _mediator.Send(command, ct);
            TempData["SuccessMessage"] = "Snippet updated.";
            return RedirectToPage("Index");
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (FluentValidation.ValidationException ex)
        {
            foreach (var failure in ex.Errors)
                ModelState.AddModelError($"Input.{failure.PropertyName}", failure.ErrorMessage);
            return Page();
        }
    }

    public sealed class InputModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Title { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Language { get; set; }

        [Required]
        public string? Code { get; set; }

        [Required]
        [MaxLength(50)]
        public string? TagName { get; set; }
    }
}
