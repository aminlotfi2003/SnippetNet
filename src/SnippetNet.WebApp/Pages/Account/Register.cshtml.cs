using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Identity.Commands.Register;
using SnippetNet.Application.Identity.Dtos;
using SnippetNet.Domain.Identity;

namespace SnippetNet.WebApp.Pages.Account;

public class RegisterModel : PageModel
{
    private const string RefreshTokenCookieName = "SnippetNet.RefreshToken";

    private readonly IMediator _mediator;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public RegisterModel(IMediator mediator, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
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
            var birthDate = DateTime.SpecifyKind(Input.BirthDate!.Value, DateTimeKind.Utc);
            var command = new RegisterUserCommand(
                Input.Email!,
                Input.Password!,
                Input.ConfirmPassword!,
                Input.FirstName!,
                Input.LastName!,
                new DateTimeOffset(birthDate));

            var result = await _mediator.Send(command, ct);

            await SignInUserAsync(Input.Email!, result, ct);

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

    private async Task SignInUserAsync(string email, AuthenticationResultDto authResult, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(email)
            ?? throw new InvalidOperationException("Unable to locate user after registration.");

        await _signInManager.SignInAsync(user, isPersistent: false);
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
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password confirmation does not match the provided password.")]
        public string? ConfirmPassword { get; set; }

        [Required]
        [MaxLength(100)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string? LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
    }
}
