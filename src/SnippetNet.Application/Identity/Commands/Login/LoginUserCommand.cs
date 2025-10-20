using MediatR;
using SnippetNet.Application.Identity.Dtos;

namespace SnippetNet.Application.Identity.Commands.Login;

public sealed record LoginUserCommand(
    string Email,
    string Password,
    string? IpAddress,
    string? UserAgent
) : IRequest<LoginResultDto>;
