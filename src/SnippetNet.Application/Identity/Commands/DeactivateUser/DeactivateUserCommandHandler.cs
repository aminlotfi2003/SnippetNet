using MediatR;
using Microsoft.AspNetCore.Identity;
using SnippetNet.Application.Common.Abstractions.Repositories.Identity;
using SnippetNet.Application.Identity.Dtos;
using SnippetNet.Domain.Identity;

namespace SnippetNet.Application.Identity.Commands.DeactivateUser;

public sealed class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, ApplicationUserDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRefreshTokenRepository _refreshTokens;

    public DeactivateUserCommandHandler(
        UserManager<ApplicationUser> userManager,
        IRefreshTokenRepository refreshTokens)
    {
        _userManager = userManager;
        _refreshTokens = refreshTokens;
    }

    public async Task<ApplicationUserDto> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            throw new InvalidOperationException("User not found.");

        if (user.IsActived)
        {
            user.IsActived = false;
            user.LockoutEnd = DateTimeOffset.MaxValue;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var description = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Unable to deactivate user: {description}");
            }

            await _refreshTokens.RevokeUserTokensAsync(user.Id, cancellationToken);
            await _refreshTokens.SaveChangesAsync(cancellationToken);
        }

        return ApplicationUserDto.FromEntity(user);
    }
}
