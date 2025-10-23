using MediatR;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Identity.Commands.PasswordReset;

namespace SnippetNet.WebApp.Pages.Account;

public class ResetPasswordModel : PageModel
{
    private readonly IMediator _mediator;

    public ResetPasswordModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    [BindProperty(SupportsGet = true)]
    public Guid? ResetId { get; set; }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public bool ResetCompleted { get; private set; }

    public IActionResult OnGet()
    {
        if (ResetId is null || ResetId == Guid.Empty)
            return RedirectToPage("ForgotPassword");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (ResetId is null || ResetId == Guid.Empty)
        {
            ModelState.AddModelError(string.Empty, "Invalid or expired reset request.");
            return Page();
        }

        if (!ModelState.IsValid)
            return Page();

        try
        {
            await _mediator.Send(new ResetPasswordWithCodeCommand(ResetId.Value, Input.Password!), ct);
            ResetCompleted = true;
            ModelState.Clear();
            Input = new InputModel();
            return Page();
        }
        catch (FluentValidation.ValidationException ex)
        {
            foreach (var failure in ex.Errors)
            {
                var key = failure.PropertyName switch
                {
                    nameof(ResetPasswordWithCodeCommand.ResetCodeId) => string.Empty,
                    nameof(ResetPasswordWithCodeCommand.NewPassword) => $"Input.{nameof(Input.Password)}",
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
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password confirmation does not match the provided password.")]
        public string? ConfirmPassword { get; set; }
    }
}
