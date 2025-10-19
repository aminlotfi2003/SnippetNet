using Microsoft.EntityFrameworkCore;
using SnippetNet.Infrastructure.Persistence;

namespace SnippetNet.Infrastructure.Extensions;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(config.GetConnectionString("Default")));

        return services;
    }
}
