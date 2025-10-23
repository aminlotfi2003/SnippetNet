using MediatR;
using Microsoft.AspNetCore.Identity;
using SnippetNet.Application.Common.Abstractions.Repositories.Identity;
using SnippetNet.Application.Common.Services.Identity;
using SnippetNet.Application.Identity.Dtos;
using SnippetNet.Domain.Identity;

namespace SnippetNet.Application.Identity.Commands.PasswordReset;

public sealed class VerifyPasswordResetCodeCommandHandler : IRequestHandler<VerifyPasswordResetCodeCommand, PasswordResetVerificationResultDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPasswordResetCodeRepository _resetCodes;
    private readonly IDateTimeProvider _clock;

    public VerifyPasswordResetCodeCommandHandler(
        UserManager<ApplicationUser> userManager,
        IPasswordResetCodeRepository resetCodes,
        IDateTimeProvider clock)
    {
        _userManager = userManager;
        _resetCodes = resetCodes;
        _clock = clock;
    }

    public async Task<PasswordResetVerificationResultDto> Handle(VerifyPasswordResetCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            throw new InvalidOperationException("Invalid verification code.");

        var codeHash = PasswordResetCode.ComputeHash(request.Code);
        var resetCode = await _resetCodes.FindPendingAsync(user.Id, codeHash, _clock.UtcNow, cancellationToken);
        if (resetCode is null)
            throw new InvalidOperationException("Invalid verification code.");

        resetCode.MarkVerified(_clock.UtcNow);
        await _resetCodes.SaveChangesAsync(cancellationToken);

        return new PasswordResetVerificationResultDto(resetCode.Id);
    }
}
