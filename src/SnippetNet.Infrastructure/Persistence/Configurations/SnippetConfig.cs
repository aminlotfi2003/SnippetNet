using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SnippetNet.Domain.Entities;

namespace SnippetNet.Infrastructure.Persistence.Configurations;

public class SnippetConfig : IEntityTypeConfiguration<Snippet>
{
    public void Configure(EntityTypeBuilder<Snippet> b)
    {
        b.ToTable("Snippets");
        b.HasKey(x => x.Id);

        b.Property(x => x.Title).IsRequired().HasMaxLength(200);
        b.Property(x => x.Language).IsRequired().HasMaxLength(50);
        b.Property(x => x.Description).HasMaxLength(2000);
        b.Property(x => x.Code).IsRequired();
        b.Property(x => x.TagName).IsRequired().HasMaxLength(50);

        b.HasIndex(x => new { x.TagName, x.Language });
    }
}
