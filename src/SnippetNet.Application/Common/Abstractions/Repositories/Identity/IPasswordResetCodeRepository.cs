using SnippetNet.Domain.Identity;

namespace SnippetNet.Application.Common.Abstractions.Repositories.Identity;

public interface IPasswordResetCodeRepository
{
    Task AddAsync(PasswordResetCode code, CancellationToken cancellationToken = default);
    Task DeleteExistingAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<PasswordResetCode?> FindPendingAsync(Guid userId, string codeHash, DateTimeOffset currentTime, CancellationToken cancellationToken = default);
    Task<PasswordResetCode?> GetByIdAsync(Guid codeId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
