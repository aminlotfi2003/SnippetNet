using MediatR;
using SnippetNet.Application.Identity.Dtos;

namespace SnippetNet.Application.Identity.Commands.TwoFactor.GenerateEmailTwoFactorToken;

public sealed record GenerateEmailTwoFactorTokenCommand(Guid UserId) : IRequest<TwoFactorTokenDto>;
