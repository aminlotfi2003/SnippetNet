using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnippetNet.Domain.Identity;

namespace SnippetNet.Infrastructure.Persistence.Configurations.Identity;

public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(x => x.Id);

        // Property
        builder.Property(x => x.TokenHash).HasMaxLength(1024).IsRequired();
        builder.Property(x => x.ExpiresAt).IsRequired();
        builder.Property(x => x.Revoked).IsRequired();

        // Index
        builder.HasIndex(x => x.ExpiresAt);
        builder.HasIndex(x => x.Revoked);

        // Relation
        builder.HasOne(x => x.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(x => x.UserId);
    }
}
