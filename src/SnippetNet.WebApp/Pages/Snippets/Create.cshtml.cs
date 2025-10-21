using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Common.Exceptions;
using SnippetNet.Application.Snippets.Commands.CreateSnippet;

namespace SnippetNet.WebApp.Pages.Snippets;

[Authorize]
public class CreateModel : PageModel
{
    private readonly IMediator _mediator;

    public CreateModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var ownerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var command = new CreateSnippetCommand(
                ownerId,
                Input.Title!,
                Input.Description,
                Input.Language!,
                Input.Code!,
                Input.TagName!);

            await _mediator.Send(command, ct);

            TempData["SuccessMessage"] = "Snippet created successfully.";
            return RedirectToPage("Index");
        }
        catch (ConflictException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
        catch (FluentValidation.ValidationException ex)
        {
            foreach (var failure in ex.Errors)
                ModelState.AddModelError($"Input.{failure.PropertyName}", failure.ErrorMessage);
            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Unexpected error: {ex.Message}");
            return Page();
        }
    }

    public sealed class InputModel
    {
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
