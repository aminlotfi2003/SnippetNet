using MediatR;

namespace SnippetNet.Application.Identity.Commands.TwoFactor.EnableEmailTwoFactor;

public sealed record EnableEmailTwoFactorCommand(Guid UserId, string Token) : IRequest;
