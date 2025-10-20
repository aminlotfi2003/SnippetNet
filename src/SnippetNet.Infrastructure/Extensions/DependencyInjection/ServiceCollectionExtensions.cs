using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SnippetNet.Application.Common.Abstractions.Repositories;
using SnippetNet.Application.Common.Abstractions.UoW;
using SnippetNet.Infrastructure.Persistence.Contexts;
using SnippetNet.Infrastructure.Persistence.Repositories;
using SnippetNet.Infrastructure.Persistence.UoW;

namespace SnippetNet.Infrastructure.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseSqlServer(config.GetConnectionString("Default")));

        // UoW & Repositories
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<ISnippetRepository, SnippetRepository>();

        return services;
    }
}
