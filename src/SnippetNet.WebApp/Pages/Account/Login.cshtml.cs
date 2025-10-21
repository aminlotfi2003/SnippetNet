using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Identity.Commands.Login;
using SnippetNet.Application.Identity.Dtos;
using SnippetNet.Domain.Identity;

namespace SnippetNet.WebApp.Pages.Account;

public class LoginModel : PageModel
{
    private const string RefreshTokenCookieName = "SnippetNet.RefreshToken";

    private readonly IMediator _mediator;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LoginModel(IMediator mediator, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _mediator = mediator;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

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
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers.UserAgent.ToString();

            var result = await _mediator.Send(new LoginUserCommand(Input.Email!, Input.Password!, ipAddress, userAgent), ct);

            if (result.RequiresTwoFactor)
            {
                TempData["TwoFactorUserId"] = result.UserId?.ToString();
                TempData["TwoFactorToken"] = result.TwoFactorToken;
                TempData["RememberMe"] = Input.RememberMe.ToString();
                TempData["ReturnUrl"] = GetSafeReturnUrl(ReturnUrl);
                return RedirectToPage("VerifyTwoFactor");
            }

            if (result.AuthenticationResult is null)
            {
                ModelState.AddModelError(string.Empty, "Unexpected authentication result.");
                return Page();
            }

            await SignInUserAsync(Input.Email!, Input.RememberMe, result.AuthenticationResult, ct);

            return LocalRedirect(GetSafeReturnUrl(ReturnUrl));
        }
        catch (FluentValidation.ValidationException ex)
        {
            foreach (var failure in ex.Errors)
                ModelState.AddModelError($"Input.{failure.PropertyName}", failure.ErrorMessage);
            return Page();
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }

    private string GetSafeReturnUrl(string? returnUrl)
        => string.IsNullOrWhiteSpace(returnUrl) || !Url.IsLocalUrl(returnUrl)
            ? Url.Content("~/")
            : returnUrl!;

    private async Task SignInUserAsync(string email, bool rememberMe, AuthenticationResultDto authResult, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(email)
            ?? throw new InvalidOperationException("Unable to locate user after successful authentication.");

        await _signInManager.SignInAsync(user, rememberMe);
        IssueRefreshTokenCookie(authResult);
    }

    private void IssueRefreshTokenCookie(AuthenticationResultDto authResult)
    {
        Response.Cookies.Append(
            RefreshTokenCookieName,
            authResult.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = authResult.RefreshTokenExpiresAt.UtcDateTime
            });

        // Store access token claims inside the current principal for convenience
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(authResult.AccessToken);
        var identity = new ClaimsIdentity(token.Claims, IdentityConstants.ApplicationScheme);
        HttpContext.User.AddIdentity(identity);
    }

    public sealed class InputModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
