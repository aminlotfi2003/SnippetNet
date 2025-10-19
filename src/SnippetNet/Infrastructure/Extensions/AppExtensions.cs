using FluentValidation;
using SnippetNet.Application.Abstractions;
using SnippetNet.Infrastructure.Persistence;
using SnippetNet.Infrastructure.Persistence.Repositories;
using SnippetNet.Infrastructure.Persistence.Seed;
using System.Reflection;

namespace SnippetNet.Infrastructure.Extensions;

public static class AppExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
    {
        // Register CQRS
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // Register Mapping
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Register Validations
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // UoW & Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ISnippetRepository, SnippetRepository>();
        services.AddScoped<ITagRepository, TagRepository>();

        // Seed options + seeder
        services.Configure<SeedOptions>(config.GetSection("Seed"));
        services.AddScoped<IdentitySeeder>();

        return services;
    }
}
