using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Identity.Commands.TwoFactor.VerifyTwoFactorLogin;
using SnippetNet.Application.Identity.Dtos;
using SnippetNet.Domain.Identity;

namespace SnippetNet.WebApp.Pages.Account;

public class VerifyTwoFactorModel : PageModel
{
    private const string RefreshTokenCookieName = "SnippetNet.RefreshToken";

    private readonly IMediator _mediator;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public VerifyTwoFactorModel(IMediator mediator, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _mediator = mediator;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? GeneratedToken { get; private set; }

    public IActionResult OnGet()
    {
        if (TempData.Peek("TwoFactorUserId") is not string userIdString || !Guid.TryParse(userIdString, out var userId))
            return RedirectToPage("Login");

        Input.UserId = userId;
        Input.RememberMe = bool.TryParse(TempData.Peek("RememberMe") as string, out var remember) && remember;
        Input.ReturnUrl = TempData.Peek("ReturnUrl") as string ?? Url.Content("~/");
        GeneratedToken = TempData.Peek("TwoFactorToken") as string;
        TempData.Keep();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers.UserAgent.ToString();

            var authResult = await _mediator.Send(
                new VerifyTwoFactorLoginCommand(Input.UserId, Input.Token!, ipAddress, userAgent),
                ct);

            await SignInUserAsync(Input.UserId, Input.RememberMe, authResult, ct);
            return LocalRedirect(GetSafeReturnUrl(Input.ReturnUrl));
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

    private async Task SignInUserAsync(Guid userId, bool rememberMe, AuthenticationResultDto authResult, CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new InvalidOperationException("Unable to locate user after two-factor verification.");

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

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(authResult.AccessToken);
        var identity = new ClaimsIdentity(token.Claims, IdentityConstants.ApplicationScheme);
        HttpContext.User.AddIdentity(identity);
    }

    public sealed class InputModel
    {
        [HiddenInput]
        public Guid UserId { get; set; }

        [Required]
        [Display(Name = "Verification code")]
        public string? Token { get; set; }

        [HiddenInput]
        public bool RememberMe { get; set; }

        [HiddenInput]
        public string? ReturnUrl { get; set; }
    }
}
