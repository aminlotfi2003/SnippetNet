using MediatR;

namespace SnippetNet.Application.Identity.Commands.PasswordReset;

public sealed record ResetPasswordWithCodeCommand(Guid ResetCodeId, string NewPassword) : IRequest<Unit>;
