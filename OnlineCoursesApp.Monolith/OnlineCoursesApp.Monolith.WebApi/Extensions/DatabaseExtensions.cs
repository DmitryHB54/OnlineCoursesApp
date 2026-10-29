using Microsoft.EntityFrameworkCore;
using OnlineCoursesApp.Monolith.Infrastructure.Persistence;

namespace OnlineCoursesApp.Monolith.WebApi.Extensions;

public static class DatabaseExtensions
{
    public static async Task ApplyMigraionsAsync(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.MigrateAsync();
        }
    }
}