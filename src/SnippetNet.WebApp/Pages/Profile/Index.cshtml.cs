using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnippetNet.Application.Identity.Dtos;
using SnippetNet.Application.Identity.Queries.GetUserById;
using SnippetNet.Application.Identity.Queries.GetUserLoginHistory;

namespace SnippetNet.WebApp.Pages.Profile;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IMediator _mediator;

    public IndexModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    public ApplicationUserDto? UserProfile { get; private set; }
    public IReadOnlyCollection<UserLoginHistoryDto> LoginHistory { get; private set; } = Array.Empty<UserLoginHistoryDto>();

    public async Task OnGetAsync(CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        UserProfile = await _mediator.Send(new GetUserByIdQuery(userId), ct);
        LoginHistory = await _mediator.Send(new GetUserLoginHistoryQuery(userId, 10), ct);
    }
}
