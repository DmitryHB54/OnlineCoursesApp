using OnlineCoursesApp.Monolith.Application;
using OnlineCoursesApp.Monolith.Infrastructure;
using OnlineCoursesApp.Monolith.WebApi.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(cfg =>
    {
        cfg.SwaggerEndpoint("swagger/v1/swagger/json", "Identity API V1");
        cfg.RoutePrefix = string.Empty;
    });
}

app.UseExceptionHandler();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();