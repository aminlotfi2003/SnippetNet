using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SnippetNet.Application.Common.Behaviors;
using System.Reflection;

namespace SnippetNet.Application.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register CQRS
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // Register Validations
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Register Behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
