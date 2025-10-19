using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SnippetNet.Infrastructure.Persistence;
using SnippetNet.Infrastructure.Persistence.Seed;

namespace SnippetNet.Infrastructure.Extensions;

public static class HostExtensions
{
    public static async Task ApplyMigrationsAndSeedAsync(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();

        // bind config
        var config = scope.ServiceProvider.GetRequiredService<IOptions<SeedOptions>>();
        var seeder = scope.ServiceProvider.GetRequiredService<IdentitySeeder>();
        await seeder.SeedAsync();
    }
}
