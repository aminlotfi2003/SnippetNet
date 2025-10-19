using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SnippetNet.Domain.Entities;
using SnippetNet.Infrastructure.Extensions;

namespace SnippetNet.Infrastructure.Persistence;

public class AppDbContext
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Snippet> Snippets => Set<Snippet>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<SnippetTag> SnippetTags => Set<SnippetTag>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.MapIdentityTables();
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        foreach (var entity in builder.Model.GetEntityTypes()
                     .Where(t => typeof(Domain.Common.EntityBase).IsAssignableFrom(t.ClrType)))
        {
            builder.Entity(entity.ClrType).Property(nameof(Domain.Common.EntityBase.CreatedAt))
                .HasDefaultValueSql("SYSUTCDATETIME()");
            builder.Entity(entity.ClrType).Property(nameof(Domain.Common.EntityBase.UpdatedAt))
                .HasDefaultValueSql("SYSUTCDATETIME()");
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
        foreach (var entry in ChangeTracker.Entries<Domain.Common.EntityBase>())
        {
            if (entry.State == EntityState.Modified)
                entry.Entity.UpdatedAt = now;
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = entry.Entity.CreatedAt == default ? now : entry.Entity.CreatedAt;
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}

public class ApplicationUser : IdentityUser<Guid>
{
}
