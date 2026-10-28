using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OnlineCoursesApp.Monolith.Application.Common.Behaviors;
using System.Reflection;

namespace OnlineCoursesApp.Monolith.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}