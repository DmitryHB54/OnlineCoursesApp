using Microsoft.AspNetCore.Http.Features;
using OnlineCoursesApp.Monolith.Application.AppConstants;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.WebApi.Services;

namespace OnlineCoursesApp.Monolith.WebApi.Extension;

public static class WebServicesExtensions
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.Configure<FormOptions>(o =>
        {
            o.MultipartBodyLengthLimit = FilesConstants.MaxFileSizeBytes;
        });

        services.AddControllers();

        return services;
    }
}