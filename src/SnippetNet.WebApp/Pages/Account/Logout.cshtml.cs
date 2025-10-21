using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Identity.Commands.Logout;
using SnippetNet.Domain.Identity;

namespace SnippetNet.WebApp.Pages.Account;

[Authorize]
public class LogoutModel : PageModel
{
    private const string RefreshTokenCookieName = "SnippetNet.RefreshToken";

    private readonly IMediator _mediator;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LogoutModel(IMediator mediator, SignInManager<ApplicationUser> signInManager)
    {
        _mediator = mediator;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (Request.Cookies.TryGetValue(RefreshTokenCookieName, out var refreshToken) && !string.IsNullOrWhiteSpace(refreshToken))
        {
            await _mediator.Send(new LogoutUserCommand(refreshToken), ct);
        }

        Response.Cookies.Delete(RefreshTokenCookieName);
        await _signInManager.SignOutAsync();
        return RedirectToPage("/Index");
    }
}
