using Microsoft.EntityFrameworkCore;
using SnippetNet.Application.Common.Abstractions.Repositories.Identity;
using SnippetNet.Domain.Identity;
using SnippetNet.Infrastructure.Persistence.Contexts;

namespace SnippetNet.Infrastructure.Persistence.Repositories.Identity;

public sealed class PasswordResetCodeRepository(ApplicationDbContext context) : IPasswordResetCodeRepository
{
    private readonly ApplicationDbContext _context = context;

    public Task AddAsync(PasswordResetCode code, CancellationToken cancellationToken = default)
        => _context.PasswordResetCodes.AddAsync(code, cancellationToken).AsTask();

    public async Task DeleteExistingAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var existing = await _context.PasswordResetCodes
            .Where(c => c.UserId == userId && c.UsedAt == null)
            .ToListAsync(cancellationToken);

        if (existing.Count == 0)
            return;

        _context.PasswordResetCodes.RemoveRange(existing);
    }

    public Task<PasswordResetCode?> FindPendingAsync(Guid userId, string codeHash, DateTimeOffset currentTime, CancellationToken cancellationToken = default)
        => _context.PasswordResetCodes
            .Where(c => c.UserId == userId
                        && c.CodeHash == codeHash
                        && c.VerifiedAt == null
                        && c.UsedAt == null
                        && c.ExpiresAt >= currentTime)
            .OrderByDescending(c => c.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<PasswordResetCode?> GetByIdAsync(Guid codeId, CancellationToken cancellationToken = default)
        => _context.PasswordResetCodes.FirstOrDefaultAsync(c => c.Id == codeId, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}
