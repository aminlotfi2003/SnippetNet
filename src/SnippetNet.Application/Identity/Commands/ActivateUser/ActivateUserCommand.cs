using FluentValidation;
using MediatR;
using SnippetNet.Application.Identity.Dtos;

namespace SnippetNet.Application.Identity.Commands.ActivateUser;

public sealed record ActivateUserCommand(Guid UserId) : IRequest<ApplicationUserDto>;
