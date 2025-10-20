using MediatR;
using SnippetNet.Application.Identity.Dtos;

namespace SnippetNet.Application.Identity.Queries.ListUsers;

public sealed record ListUsersQuery(bool IncludeInactive = true) : IRequest<IReadOnlyCollection<ApplicationUserDto>>;
