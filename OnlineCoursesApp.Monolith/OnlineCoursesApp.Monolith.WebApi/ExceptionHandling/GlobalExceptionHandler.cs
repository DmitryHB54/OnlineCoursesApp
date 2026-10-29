using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineCoursesApp.Monolith.Domain.Exceptions;

namespace OnlineCoursesApp.Monolith.WebApi.ExceptionHandling;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        var problemDetails = exception switch
        {
            ValidationException validationException => CreateValidationProblem(validationException),
            NotFoundException notFoundException => CreateNotFoundProblem(notFoundException),
            ForbiddenException forbiddenException => CreateForbiddenProblem(forbiddenException),
            _ => CreateServerErrorProblem()
        };

        httpContext.Response.Clear();
        httpContext.Response.StatusCode = problemDetails.Status ?? 500;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static ProblemDetails CreateValidationProblem(ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray());

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "One or more validation errors occured.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        problemDetails.Extensions["errors"] = errors;

        return problemDetails;
    }

    private static ProblemDetails CreateNotFoundProblem(NotFoundException exception)
    {
        return new()
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Resource not found",
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
        };
    }

    private static ProblemDetails CreateForbiddenProblem(ForbiddenException exception)
    {
        return new()
        {
            Status = StatusCodes.Status403Forbidden,
            Title = "Access denied",
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
        };
    }

    private static ProblemDetails CreateServerErrorProblem()
    {
        return new()
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occured.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };
    }
}