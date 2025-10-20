using MediatR;
using SnippetNet.Application.Common.Abstractions.Repositories.Identity;
using SnippetNet.Application.Identity.Dtos;

namespace SnippetNet.Application.Identity.Queries.GetUserLoginHistory;

public sealed class GetUserLoginHistoryQueryHandler : IRequestHandler<GetUserLoginHistoryQuery, IReadOnlyCollection<UserLoginHistoryDto>>
{
    private readonly IUserLoginHistoryRepository _loginHistories;

    public GetUserLoginHistoryQueryHandler(IUserLoginHistoryRepository loginHistories)
    {
        _loginHistories = loginHistories;
    }

    public async Task<IReadOnlyCollection<UserLoginHistoryDto>> Handle(GetUserLoginHistoryQuery request, CancellationToken cancellationToken)
    {
        var histories = await _loginHistories.GetRecentAsync(request.UserId, request.Count, cancellationToken);
        return histories
            .Select(UserLoginHistoryDto.FromEntity)
            .ToList();
    }
}
