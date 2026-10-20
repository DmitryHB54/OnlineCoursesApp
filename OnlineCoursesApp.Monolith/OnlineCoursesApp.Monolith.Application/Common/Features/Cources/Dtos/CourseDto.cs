using OnlineCoursesApp.Monolith.Domain.Enums;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;

public record CourseDto(
    Guid Id,
    string Title,
    string Description,
    Guid AuthorId,
    CourseCategory Category,
    decimal Price,
    DateTime CreatedAt);