using MediatR;

namespace SnippetNet.Application.Identity.Commands.Logout;

public sealed record LogoutUserCommand(string RefreshToken) : IRequest;

