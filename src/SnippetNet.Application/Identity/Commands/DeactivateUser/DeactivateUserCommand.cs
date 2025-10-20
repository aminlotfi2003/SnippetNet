using MediatR;
using SnippetNet.Application.Identity.Dtos;

namespace SnippetNet.Application.Identity.Commands.DeactivateUser;

public sealed record DeactivateUserCommand(Guid UserId) : IRequest<ApplicationUserDto>;
