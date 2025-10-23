using MediatR;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Identity.Commands.PasswordReset;
using SnippetNet.Application.Identity.Dtos;

namespace SnippetNet.WebApp.Pages.Account;

public class VerifyResetCodeModel : PageModel
{
    private readonly IMediator _mediator;

    public VerifyResetCodeModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public IActionResult OnGet(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return RedirectToPage("ForgotPassword");

        Input.Email = email;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            PasswordResetVerificationResultDto result = await _mediator.Send(
                new VerifyPasswordResetCodeCommand(Input.Email!, Input.Code!),
                ct);

            return RedirectToPage("ResetPassword", new { resetId = result.ResetCodeId });
        }
        catch (FluentValidation.ValidationException ex)
        {
            foreach (var failure in ex.Errors)
            {
                var key = failure.PropertyName switch
                {
                    nameof(Input.Email) => $"Input.{nameof(Input.Email)}",
                    nameof(Input.Code) => $"Input.{nameof(Input.Code)}",
                    _ => string.Empty
                };

                ModelState.AddModelError(key, failure.ErrorMessage);
            }
            return Page();
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }

    public sealed class InputModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "The verification code must be a 6-digit number.")]
        public string? Code { get; set; }
    }
}
