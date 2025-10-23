using MediatR;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Identity.Commands.ForgotPassword;

namespace SnippetNet.WebApp.Pages.Account;

public class ForgotPasswordModel : PageModel
{
    private readonly IMediator _mediator;

    public ForgotPasswordModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            await _mediator.Send(new ForgotPasswordCommand(Input.Email!), ct);
            return RedirectToPage("VerifyResetCode", new { email = Input.Email });
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
        [EmailAddress]
        public string? Email { get; set; }
    }
}
