using MediatR;
using SnippetNet.Application.Identity.Dtos;

namespace SnippetNet.Application.Identity.Queries.GetUserLoginHistory;

public sealed record GetUserLoginHistoryQuery(Guid UserId, int Count = 10) : IRequest<IReadOnlyCollection<UserLoginHistoryDto>>;
