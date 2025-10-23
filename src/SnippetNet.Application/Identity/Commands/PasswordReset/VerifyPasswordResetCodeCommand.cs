using MediatR;
using SnippetNet.Application.Identity.Dtos;

namespace SnippetNet.Application.Identity.Commands.PasswordReset;

public sealed record VerifyPasswordResetCodeCommand(string Email, string Code) : IRequest<PasswordResetVerificationResultDto>;
