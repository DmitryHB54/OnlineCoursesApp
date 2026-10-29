using OnlineCoursesApp.Monolith.Application;
using OnlineCoursesApp.Monolith.Infrastructure;
using OnlineCoursesApp.Monolith.WebApi.Extensions;
using OnlineCoursesApp.Monolith.WebApi.Extension;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAuthConfig(builder.Configuration);
builder.Services.AddWebServices();

builder.Services.AddGlobalExceptionHandling();
builder.Services.AddSwaggerConfig();

var app = builder.Build();

await app.ApplyMigraionsAsync();

app.UseExceptionHandler();
app.UseSwaggerDocumentation(app.Environment);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();