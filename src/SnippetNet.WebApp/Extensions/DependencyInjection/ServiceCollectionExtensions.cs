using SnippetNet.Application.Extensions.DependencyInjection;
using SnippetNet.Infrastructure.Extensions.DependencyInjection;

namespace SnippetNet.WebApp.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        // Register Dependencies Layers
        services.AddApplication()
                .AddPersistence(config);

        return services;
    }
}
