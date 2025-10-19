using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnippetNet.Domain.Entities;

namespace SnippetNet.Infrastructure.Persistence.Configurations;

public class SnippetTagConfig : IEntityTypeConfiguration<SnippetTag>
{
    public void Configure(EntityTypeBuilder<SnippetTag> b)
    {
        b.ToTable("SnippetTags");
        b.HasKey(x => new { x.SnippetId, x.TagId });

        b.HasOne(x => x.Snippet)
         .WithMany(s => s.SnippetTags)
         .HasForeignKey(x => x.SnippetId);

        b.HasOne(x => x.Tag)
         .WithMany(t => t.SnippetTags)
         .HasForeignKey(x => x.TagId);
    }
}
