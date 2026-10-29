using OnlineCoursesApp.Monolith.WebApi.ExceptionHandling;

namespace OnlineCoursesApp.Monolith.WebApi.Extensions;

public static class ExceptionHandlingExtensions
{
    public static IServiceCollection AddGlobalExceptionHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}