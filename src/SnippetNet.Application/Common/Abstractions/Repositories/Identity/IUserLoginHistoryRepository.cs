using SnippetNet.Domain.Identity;

namespace SnippetNet.Application.Common.Abstractions.Repositories.Identity;

public interface IUserLoginHistoryRepository
{
    Task<IReadOnlyList<UserLoginHistory>> GetRecentAsync(Guid userId, int count, CancellationToken cancellationToken = default);
    Task AddAsync(UserLoginHistory history, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
