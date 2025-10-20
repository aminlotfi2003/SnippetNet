using MediatR;
using SnippetNet.Application.Identity.Dtos;

namespace SnippetNet.Application.Identity.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId) : IRequest<ApplicationUserDto?>;
