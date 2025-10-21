using MediatR;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Identity.Commands.ForgotPassword;
using SnippetNet.Application.Identity.Dtos;

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

    public ForgotPasswordTokenDto? Result { get; private set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            Result = await _mediator.Send(new ForgotPasswordCommand(Input.Email!), ct);

            if (Result.Success && Result.ResetToken is not null)
                ViewData["ResetToken"] = Result.ResetToken;

            ViewData["Email"] = Input.Email;
            return Page();
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
