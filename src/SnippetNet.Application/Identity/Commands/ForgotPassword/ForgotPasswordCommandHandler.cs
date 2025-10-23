using System.Security.Cryptography;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SnippetNet.Application.Common.Abstractions.Repositories.Identity;
using SnippetNet.Application.Common.Services.Identity;
using SnippetNet.Application.Common.Services.Notifications;
using SnippetNet.Application.Identity.Dtos;
using SnippetNet.Domain.Identity;

namespace SnippetNet.Application.Identity.Commands.ForgotPassword;

public sealed class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ForgotPasswordResultDto>
{
    private static readonly TimeSpan CodeLifetime = TimeSpan.FromMinutes(10);
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPasswordResetCodeRepository _resetCodes;
    private readonly IDateTimeProvider _clock;
    private readonly IEmailSender _email;

    public ForgotPasswordCommandHandler(
        UserManager<ApplicationUser> userManager,
        IPasswordResetCodeRepository resetCodes,
        IDateTimeProvider clock,
        IEmailSender email)
    {
        _userManager = userManager;
        _resetCodes = resetCodes;
        _clock = clock;
        _email = email;
    }

    public async Task<ForgotPasswordResultDto> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || string.IsNullOrWhiteSpace(user.Email))
            return ForgotPasswordResultDto.Succeeded();

        var code = GenerateCode();
        var codeHash = PasswordResetCode.ComputeHash(code);
        var expiresAt = _clock.UtcNow.Add(CodeLifetime);

        await _resetCodes.DeleteExistingAsync(user.Id, cancellationToken);
        var resetCode = PasswordResetCode.Create(user.Id, codeHash, expiresAt);
        await _resetCodes.AddAsync(resetCode, cancellationToken);
        await _resetCodes.SaveChangesAsync(cancellationToken);

        return ForgotPasswordResultDto.Succeeded();
    }

    private static string GenerateCode()
    {
        var value = RandomNumberGenerator.GetInt32(0, 1_000_000);
        return value.ToString("D6");
    }
}
