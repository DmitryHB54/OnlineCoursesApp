namespace OnlineCoursesApp.Monolith.Application.Common.Features.Reports.Dtos;

public record CourseReportRowDto(
    string Title,
    string AuthorEmail,
    string Category,
    decimal Price,
    int MaterialsCount,
    int EnrollmentsCount);