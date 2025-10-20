﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using SnippetNet.Application.Identity.Dtos;
using SnippetNet.Domain.Identity;

namespace SnippetNet.Application.Identity.Commands.ForgotPassword;

public sealed class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ForgotPasswordTokenDto>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ForgotPasswordCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ForgotPasswordTokenDto> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return ForgotPasswordTokenDto.SuccessWithoutToken();

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        return ForgotPasswordTokenDto.SuccessWithToken(token);
    }
}
