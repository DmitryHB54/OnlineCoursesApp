using Microsoft.AspNetCore.Http.Features;
using OnlineCoursesApp.Monolith.Application;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Infrastructure;
using OnlineCoursesApp.Monolith.Infrastructure.Persistence;
using OnlineCoursesApp.Monolith.WebApi.ExceptionHandling;
using OnlineCoursesApp.Monolith.WebApi.Extensions;
using OnlineCoursesApp.Monolith.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesApp.Monolith.Application.AppConstants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.Configure<FormOptions>(o =>
{
    o.MultipartBodyLengthLimit = FilesConstants.MaxFileSizeBytes;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddAuthConfig(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();

builder.Services.AddSwaggerConfig();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(cfg =>
    {
        cfg.SwaggerEndpoint("swagger/v1/swagger.json", "Online courses API V1");
        cfg.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();