﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using SnippetNet.Application.Identity.Dtos;
using SnippetNet.Domain.Identity;

namespace SnippetNet.Application.Identity.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ApplicationUserDto?>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserByIdQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        return user is null ? null : ApplicationUserDto.FromEntity(user);
    }
}
