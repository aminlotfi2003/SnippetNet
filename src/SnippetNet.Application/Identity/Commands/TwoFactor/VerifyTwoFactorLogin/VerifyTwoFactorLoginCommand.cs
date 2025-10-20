using MediatR;
using SnippetNet.Application.Identity.Dtos;

namespace SnippetNet.Application.Identity.Commands.TwoFactor.VerifyTwoFactorLogin;

public sealed record VerifyTwoFactorLoginCommand(
    Guid UserId,
    string Token,
    string? IpAddress,
    string? UserAgent
) : IRequest<AuthenticationResultDto>;
