﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using SnippetNet.Application.Identity.Dtos;
using SnippetNet.Domain.Identity;

namespace SnippetNet.Application.Identity.Commands.TwoFactor.GenerateEmailTwoFactorToken;

internal sealed class GenerateEmailTwoFactorTokenCommandHandler : IRequestHandler<GenerateEmailTwoFactorTokenCommand, TwoFactorTokenDto>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GenerateEmailTwoFactorTokenCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<TwoFactorTokenDto> Handle(GenerateEmailTwoFactorTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            throw new InvalidOperationException("User not found.");

        if (string.IsNullOrWhiteSpace(user.Email))
            throw new InvalidOperationException("User email is not configured.");

        var token = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
        return new TwoFactorTokenDto(token);
    }
}
