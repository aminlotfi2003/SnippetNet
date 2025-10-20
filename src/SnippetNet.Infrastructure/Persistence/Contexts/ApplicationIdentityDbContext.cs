using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SnippetNet.Domain.Identity;
using SnippetNet.Infrastructure.Extensions;

namespace SnippetNet.Infrastructure.Persistence.Contexts;

public class ApplicationIdentityDbContext
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid,
                        IdentityUserClaim<Guid>, IdentityUserRole<Guid>,
                        IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>,
                        IdentityUserToken<Guid>>
{
    public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : base(options) { }

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<UserPasswordHistory> UserPasswordHistories => Set<UserPasswordHistory>();
    public DbSet<UserLoginHistory> UserLoginHistories => Set<UserLoginHistory>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.MapIdentityTables();
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationIdentityDbContext).Assembly);
    }
}
