using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineCoursesApp.Monolith.Application.Auth;
using OnlineCoursesApp.Monolith.Infrastructure.Auth;
using OnlineCoursesApp.Monolith.Infrastructure.Persistence;

namespace OnlineCoursesApp.Monolith.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("Default")));

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}