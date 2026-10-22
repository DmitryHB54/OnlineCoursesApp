using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineCoursesApp.Monolith.Application.Auth;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Infrastructure.Auth;
using OnlineCoursesApp.Monolith.Infrastructure.Persistence;
using OnlineCoursesApp.Monolith.Infrastructure.Persistence.Decorators;
using OnlineCoursesApp.Monolith.Infrastructure.Persistence.Repositories;
using OnlineCoursesApp.Monolith.Infrastructure.Storage;

namespace OnlineCoursesApp.Monolith.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("Default")));

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.Decorate<IUnitOfWork, LoggingUnitOfWork>();

        services.AddScoped<ICourseRepository, CourseRepository>();
        services.Decorate<ICourseRepository, LoggingCourseRepository>();
        services.Decorate<ICourseRepository, CachedCourseRepository>();

        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        services.AddScoped<ILessonMaterialRepository, LessonMaterialRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();

        services.Configure<StorageOptions>(configuration.GetSection(StorageOptions.SectionName));
        services.AddScoped<IFileStorageService, FileStorageService>();

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>(); 

        return services;
    }
}