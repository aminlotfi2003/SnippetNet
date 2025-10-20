using Microsoft.EntityFrameworkCore;
using SnippetNet.Application.Common.Abstractions.Repositories.Identity;
using SnippetNet.Domain.Identity;
using SnippetNet.Infrastructure.Persistence.Contexts;

namespace SnippetNet.Infrastructure.Persistence.Repositories.Identity;

public sealed class UserLoginHistoryRepository(ApplicationIdentityDbContext context) : IUserLoginHistoryRepository
{
    private readonly ApplicationIdentityDbContext _context = context;

    public async Task<IReadOnlyList<UserLoginHistory>> GetRecentAsync(
        Guid userId,
        int count,
        CancellationToken cancellationToken = default)
    {
        count = count <= 0 ? 10 : count;

        return await _context.UserLoginHistories
            .Where(history => history.UserId == userId)
            .OrderByDescending(history => history.LoggedInAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(UserLoginHistory history, CancellationToken cancellationToken = default)
    {
        await _context.UserLoginHistories.AddAsync(history, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}
