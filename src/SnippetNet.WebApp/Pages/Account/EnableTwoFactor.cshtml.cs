using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Identity.Commands.TwoFactor.EnableEmailTwoFactor;
using SnippetNet.Application.Identity.Commands.TwoFactor.GenerateEmailTwoFactorToken;
using SnippetNet.Domain.Identity;

namespace SnippetNet.WebApp.Pages.Account;

[Authorize]
public class EnableTwoFactorModel : PageModel
{
    private readonly IMediator _mediator;
    private readonly UserManager<ApplicationUser> _userManager;

    public EnableTwoFactorModel(IMediator mediator, UserManager<ApplicationUser> userManager)
    {
        _mediator = mediator;
        _userManager = userManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? GeneratedToken { get; private set; }
    public bool TwoFactorEnabled { get; private set; }
    public string? StatusMessage { get; private set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        var user = await GetCurrentUserAsync(ct);
        TwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
    }

    public async Task<IActionResult> OnPostGenerateAsync(CancellationToken ct)
    {
        var user = await GetCurrentUserAsync(ct);
        TwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
        if (TwoFactorEnabled)
        {
            StatusMessage = "Two-factor authentication is already enabled.";
            return Page();
        }

        var token = await _mediator.Send(new GenerateEmailTwoFactorTokenCommand(user.Id), ct);
        GeneratedToken = token.Token;
        StatusMessage = "Use the generated code to activate email-based two-factor authentication.";
        return Page();
    }

    public async Task<IActionResult> OnPostEnableAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync(ct);
            return Page();
        }

        var user = await GetCurrentUserAsync(ct);
        TwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
        if (TwoFactorEnabled)
        {
            StatusMessage = "Two-factor authentication is already enabled.";
            return Page();
        }

        await _mediator.Send(new EnableEmailTwoFactorCommand(user.Id, Input.Token!), ct);
        TwoFactorEnabled = true;
        StatusMessage = "Two-factor authentication successfully enabled.";
        GeneratedToken = null;
        return Page();
    }

    private async Task<ApplicationUser> GetCurrentUserAsync(CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            throw new InvalidOperationException("Authenticated user could not be found.");
        return user;
    }

    public sealed class InputModel
    {
        [Required]
        [Display(Name = "Verification code")]
        public string? Token { get; set; }
    }
}
