using MediatR;
using Microsoft.AspNetCore.Identity;
using SnippetNet.Application.Common.Abstractions.Repositories.Identity;
using SnippetNet.Application.Common.Services.Identity;
using SnippetNet.Domain.Identity;

namespace SnippetNet.Application.Identity.Commands.PasswordReset;

public sealed class ResetPasswordWithCodeCommandHandler : IRequestHandler<ResetPasswordWithCodeCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPasswordResetCodeRepository _resetCodes;
    private readonly IUserPasswordHistoryRepository _passwordHistories;
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly IDateTimeProvider _clock;

    public ResetPasswordWithCodeCommandHandler(
        UserManager<ApplicationUser> userManager,
        IPasswordResetCodeRepository resetCodes,
        IUserPasswordHistoryRepository passwordHistories,
        IRefreshTokenRepository refreshTokens,
        IDateTimeProvider clock)
    {
        _userManager = userManager;
        _resetCodes = resetCodes;
        _passwordHistories = passwordHistories;
        _refreshTokens = refreshTokens;
        _clock = clock;
    }

    public async Task<Unit> Handle(ResetPasswordWithCodeCommand request, CancellationToken cancellationToken)
    {
        var resetCode = await _resetCodes.GetByIdAsync(request.ResetCodeId, cancellationToken)
            ?? throw new InvalidOperationException("Invalid or expired reset request.");

        var now = _clock.UtcNow;
        if (resetCode.IsUsed || resetCode.VerifiedAt is null || resetCode.IsExpired(now))
            throw new InvalidOperationException("Invalid or expired reset request.");

        var user = await _userManager.FindByIdAsync(resetCode.UserId.ToString())
            ?? throw new InvalidOperationException("Invalid or expired reset request.");

        await EnsurePasswordDoesNotMatchHistoryAsync(user, request.NewPassword, cancellationToken);

        var previousPasswordHash = user.PasswordHash;
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
        if (!result.Succeeded)
        {
            var description = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Unable to reset password: {description}");
        }

        user.LastPasswordChangedAt = now;
        await _userManager.UpdateAsync(user);

        if (!string.IsNullOrEmpty(previousPasswordHash))
        {
            var historyEntry = UserPasswordHistory.Create(user.Id, previousPasswordHash!, now);
            await _passwordHistories.AddAsync(historyEntry, cancellationToken);
            await _passwordHistories.PruneExcessAsync(user.Id, 5, cancellationToken);
            await _passwordHistories.SaveChangesAsync(cancellationToken);
        }

        await _refreshTokens.RevokeUserTokensAsync(user.Id, cancellationToken);
        await _refreshTokens.SaveChangesAsync(cancellationToken);

        resetCode.MarkUsed(now);
        await _resetCodes.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task EnsurePasswordDoesNotMatchHistoryAsync(ApplicationUser user, string newPassword, CancellationToken cancellationToken)
    {
        var recentPasswords = await _passwordHistories.GetRecentAsync(user.Id, 5, cancellationToken);

        foreach (var previous in recentPasswords)
        {
            var verification = _userManager.PasswordHasher.VerifyHashedPassword(user, previous.PasswordHash, newPassword);
            if (verification is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded)
                throw new InvalidOperationException("New password cannot match any of the last five passwords.");
        }

        if (!string.IsNullOrEmpty(user.PasswordHash))
        {
            var currentVerification = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, newPassword);
            if (currentVerification is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded)
                throw new InvalidOperationException("New password cannot match any of the last five passwords.");
        }
    }
}
