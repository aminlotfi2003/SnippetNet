using SnippetNet.Domain.Common.Base;
using System.Security.Cryptography;
using System.Text;

namespace SnippetNet.Domain.Identity;

public class PasswordResetCode : EntityBase
{
    private PasswordResetCode()
    {
    }

    public Guid UserId { get; private set; }
    public ApplicationUser User { get; private set; } = default!;
    public string CodeHash { get; private set; } = string.Empty;
    public DateTimeOffset ExpiresAt { get; private set; }
    public DateTimeOffset? VerifiedAt { get; private set; }
    public DateTimeOffset? UsedAt { get; private set; }

    public static PasswordResetCode Create(Guid userId, string codeHash, DateTimeOffset expiresAt)
        => new()
        {
            UserId = userId,
            CodeHash = codeHash,
            ExpiresAt = expiresAt
        };

    public static string ComputeHash(string code)
    {
        var bytes = Encoding.UTF8.GetBytes(code);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash);
    }

    public bool MatchesHash(string hash) => CodeHash == hash;

    public bool IsExpired(DateTimeOffset referenceTime) => referenceTime >= ExpiresAt;

    public bool IsVerified => VerifiedAt is not null;

    public bool IsUsed => UsedAt is not null;

    public void MarkVerified(DateTimeOffset timestamp)
    {
        VerifiedAt ??= timestamp;
    }

    public void MarkUsed(DateTimeOffset timestamp)
    {
        UsedAt ??= timestamp;
    }
}
